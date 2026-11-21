#!/usr/bin/env python3
"""Refresh manifest pins to newer, verified signed stable SemVer tags."""

import argparse
import base64
from collections import defaultdict
from copy import deepcopy
from datetime import datetime, timezone
from functools import cache
import json
import os
from pathlib import Path
import re
import sys
from urllib.error import HTTPError, URLError
from urllib.parse import quote
from urllib.request import Request, urlopen


ROOT = Path(__file__).resolve().parents[2]
MANIFEST = ROOT / "Functions.Templates/Template-Manifest/manifest.json"
MANIFEST_PATH = MANIFEST.relative_to(ROOT).as_posix()
STABLE_VERSION = re.compile(
    r"v?(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)"
    r"(?:\+[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*)?"
)


def version(tag):
    match = STABLE_VERSION.fullmatch(tag)
    return tuple(map(int, match.groups())) if match else None


class GitHub:
    def __init__(self, token):
        self.token = token

    @cache
    def get(self, endpoint, allow_missing=False):
        request = Request(
            f"https://api.github.com/{endpoint}",
            headers={
                "Authorization": f"Bearer {self.token}",
                "Accept": "application/vnd.github+json",
                "X-GitHub-Api-Version": "2022-11-28",
                "User-Agent": "azure-functions-manifest-updater",
            },
        )
        try:
            with urlopen(request, timeout=60) as response:
                return json.load(response)
        except HTTPError as error:
            if allow_missing and error.code == 404:
                return None
            raise RuntimeError(f"GitHub API {endpoint}: HTTP {error.code}") from error
        except URLError as error:
            raise RuntimeError(f"GitHub API {endpoint}: {error.reason}") from error

    def tags(self, repository):
        result = []
        page = 1
        while True:
            batch = self.get(f"repos/{repository}/tags?per_page=100&page={page}")
            result.extend(tag["name"] for tag in batch)
            if len(batch) < 100:
                return result
            page += 1


def refresh(manifest, github):
    updated = deepcopy(manifest)
    repositories = defaultdict(list)
    changes = []
    skipped = []
    for template in updated["templates"]:
        match = re.fullmatch(
            r"https://github\.com/([A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+)",
            template["repositoryUrl"],
        )
        if not match:
            raise ValueError(f"{template['id']}: unsupported repositoryUrl")
        repositories[match[1]].append(template)

    for repository, templates in repositories.items():
        candidates = sorted(
            ((version(tag), tag) for tag in github.tags(repository) if version(tag)),
            reverse=True,
        )
        verified = {}
        for template in templates:
            current_ref = template["gitRef"]
            current = (
                version(current_ref.removeprefix("refs/tags/"))
                if current_ref.startswith("refs/tags/")
                else None
            )
            if current is None:
                skipped.append(f"{template['id']}: preserved non-stable or non-tag pin {current_ref}")
                continue
            folder = template["folderPath"]
            if folder not in ("", ".") and (
                folder.startswith("/") or any(part in ("", ".", "..") for part in folder.split("/"))
            ):
                raise ValueError(f"{template['id']}: invalid folderPath {folder!r}")

            for candidate, tag in candidates:
                if candidate <= current:
                    continue
                encoded_tag = quote(tag, safe="")
                if tag not in verified:
                    ref = github.get(f"repos/{repository}/git/ref/tags/{encoded_tag}")
                    if ref["object"]["type"] != "tag":
                        verified[tag] = False
                        skipped.append(f"{repository}@{tag}: skipped lightweight tag")
                    else:
                        tag_object = github.get(
                            f"repos/{repository}/git/tags/{ref['object']['sha']}"
                        )
                        verified[tag] = (
                            tag_object["verification"]["verified"] is True
                            and tag_object["object"]["type"] == "commit"
                        )
                        if not verified[tag]:
                            skipped.append(f"{repository}@{tag}: skipped unverified/non-commit tag")
                if not verified[tag]:
                    continue
                content_path = "" if folder in ("", ".") else quote(folder, safe="/")
                contents = github.get(
                    f"repos/{repository}/contents/{content_path}?ref="
                    f"{quote('refs/tags/' + tag, safe='')}",
                    allow_missing=True,
                )
                if not isinstance(contents, list):
                    skipped.append(
                        f"{template['id']}@{tag}: folder {folder!r} missing or not a directory"
                    )
                    continue
                template["gitRef"] = f"refs/tags/{tag}"
                changes.append({
                    "id": template["id"],
                    "repository": repository,
                    "old": current_ref,
                    "new": template["gitRef"],
                })
                break

    if changes:
        updated["generatedAt"] = datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ")
    return updated, changes, skipped


def untagged_changes(manifest, github):
    """Find pinned sample folders whose default branch has commits after the pinned tag."""
    groups = defaultdict(lambda: {"folders": {}})
    for template in manifest["templates"]:
        if not template["gitRef"].startswith("refs/tags/"):
            continue
        repository = template["repositoryUrl"].removeprefix("https://github.com/")
        folder = template["folderPath"].strip("/")
        group = groups[(repository, template["gitRef"].removeprefix("refs/tags/"))]
        group["folders"].setdefault("" if folder == "." else folder, set()).add(template["id"])

    pending = []
    errors = []
    for (repository, tag), group in sorted(groups.items()):
        try:
            branch = github.get(f"repos/{repository}")["default_branch"]
            comparison = github.get(
                f"repos/{repository}/compare/{quote(tag, safe='')}...{quote(branch, safe='')}"
            )
        except (RuntimeError, OSError) as error:
            errors.append(f"{repository}@{tag}: could not compare with default branch ({error})")
            continue
        if comparison["ahead_by"] == 0:
            continue
        files = comparison.get("files", [])
        changed_paths = [
            path
            for file in files
            for path in (file.get("filename"), file.get("previous_filename"))
            if path
        ]
        changed = sorted(
            folder or "."
            for folder in group["folders"]
            if not folder or any(name.startswith(folder + "/") for name in changed_paths)
        )
        truncated = len(files) >= 300
        if not changed and not truncated:
            continue
        reported_folders = changed or sorted(folder or "." for folder in group["folders"])
        pending.append({
            "repository": repository,
            "tag": tag,
            "branch": branch,
            "ahead_by": comparison["ahead_by"],
            "folders": reported_folders,
            "ids": sorted({
                template_id
                for folder in reported_folders
                for template_id in group["folders"][folder if folder != "." else ""]
            }),
            "truncated": truncated,
            "compare_url": f"https://github.com/{repository}/compare/{tag}...{branch}",
        })
    return pending, errors


def write_json(path, value):
    path.write_text(json.dumps(value, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")


def untagged_report(pending, errors):
    lines = ["## Untagged sample changes: please create signed tag", ""]
    if pending:
        lines += [
            "These sample repositories have commits on their default branch after the pinned "
            "tag that touch a manifest sample folder. Owners: please review and create a new "
            "signed annotated release tag so a later run can pick it up.",
            "",
            "| Repository | Pinned tag | Commits ahead | Sample folders | Samples | Changes |",
            "| --- | --- | --- | --- | --- | --- |",
        ]
        for item in pending:
            folders = ", ".join(f"`{folder}`" for folder in item["folders"])
            if item["truncated"]:
                folders += " (file list truncated; verify manually)"
            lines.append(
                f"| `{item['repository']}` | `{item['tag']}` | {item['ahead_by']} | {folders} | "
                f"{', '.join(f'`{i}`' for i in item['ids'])} | "
                f"[`{item['tag']}...{item['branch']}`]({item['compare_url']}) |"
            )
    else:
        lines.append("No untagged sample-folder changes were found.")
    if errors:
        lines += ["", "Could not check:", ""]
        lines += [f"- {error}" for error in errors]
    return "\n".join(lines) + "\n"


def report(changes, skipped):
    lines = ["## Sample tag updates", ""]
    if changes:
        lines += ["| Sample | Repository | Previous | New |", "| --- | --- | --- | --- |"]
        lines += [
            f"| `{c['id']}` | `{c['repository']}` | `{c['old']}` | `{c['new']}` |"
            for c in changes
        ]
    else:
        lines.append("No eligible newer tags; manifest and generatedAt are unchanged.")
    if skipped:
        lines += ["", "## Skipped candidates and pins", ""]
        lines += [f"- {reason}" for reason in skipped]
    return "\n".join(lines) + "\n"


def pin_changes(base, head):
    expected = deepcopy(base)
    head_templates = {template["id"]: template for template in head["templates"]}
    changes = {}
    for template in expected["templates"]:
        proposed = head_templates.get(template["id"])
        if proposed is None:
            raise ValueError("Existing update PR removes templates; manual review required")
        if template["gitRef"] != proposed["gitRef"]:
            template["gitRef"] = proposed["gitRef"]
            changes[template["id"]] = proposed["gitRef"]
    expected["generatedAt"] = head["generatedAt"]
    if expected != head:
        raise ValueError("Existing update PR contains non-pin changes; manual review required")
    return changes


def plan_pull_request(manifest, changes, github, repository, branch):
    desired = {change["id"]: change["new"] for change in changes}
    templates = {template["id"]: template for template in manifest["templates"]}
    existing = []
    page = 1
    while True:
        pulls = github.get(
            f"repos/{repository}/pulls?state=open&base={quote(branch, safe='')}"
            f"&per_page=100&page={page}"
        )
        for pull in pulls:
            if (
                not pull["head"]["ref"].startswith("automation/manifest-tags-")
                or pull["user"]["login"] != "github-actions[bot]"
                or not pull["head"]["repo"]
                or pull["head"]["repo"]["full_name"] != repository
            ):
                continue
            files = github.get(f"repos/{repository}/pulls/{pull['number']}/files?per_page=100")
            if [file["filename"] for file in files] != [MANIFEST_PATH]:
                raise ValueError(f"PR #{pull['number']} edits other files; manual review required")
            comparison = github.get(
                f"repos/{repository}/compare/{pull['base']['sha']}...{pull['head']['sha']}"
            )
            manifests = []
            for commit in (comparison["merge_base_commit"]["sha"], pull["head"]["sha"]):
                content = github.get(f"repos/{repository}/contents/{MANIFEST_PATH}?ref={commit}")
                manifests.append(json.loads(base64.b64decode(content["content"])))
            old_changes = pin_changes(*manifests)
            old_templates = {template["id"]: template for template in manifests[1]["templates"]}
            for template_id, old_ref in old_changes.items():
                target = templates.get(template_id)
                previous = old_templates[template_id]
                old_version = version(old_ref.removeprefix("refs/tags/"))
                new_version = version(target["gitRef"].removeprefix("refs/tags/")) if target else None
                if (
                    not target
                    or target["repositoryUrl"] != previous["repositoryUrl"]
                    or target["folderPath"] != previous["folderPath"]
                    or old_version is None
                    or new_version is None
                    or new_version < old_version
                ):
                    raise ValueError(
                        f"PR #{pull['number']} has changes not covered by this update; "
                        "manual review required"
                    )
            existing.append({
                "number": pull["number"],
                "sha": pull["head"]["sha"],
                "url": pull["html_url"],
                "changes": old_changes,
            })
        if len(pulls) < 100:
            break
        page += 1
    equivalent = next((pull for pull in existing if pull["changes"] == desired), None)
    return {
        "create": equivalent is None,
        "replacement_url": equivalent["url"] if equivalent else "",
        "replacement_sha": equivalent["sha"] if equivalent else "",
        "superseded": [pull for pull in existing if pull is not equivalent],
    }


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--manifest", type=Path, default=MANIFEST)
    parser.add_argument("--report-dir", type=Path, required=True)
    parser.add_argument("--plan-pr", action="store_true")
    args = parser.parse_args()
    token = os.environ.get("GH_TOKEN")
    if not token:
        raise ValueError("GH_TOKEN is required to read sample repositories")
    manifest = json.loads(args.manifest.read_text(encoding="utf-8"))
    if args.plan_pr:
        result = json.loads((args.report_dir / "result.json").read_text(encoding="utf-8"))
        plan = plan_pull_request(
            manifest, result["changes"], GitHub(token),
            os.environ["GH_REPO"], os.environ["BASE_BRANCH"],
        )
        write_json(args.report_dir / "pr-plan.json", plan)
        with open(os.environ["GITHUB_OUTPUT"], "a", encoding="utf-8") as stream:
            stream.write(f"create_pr={'true' if plan['create'] else 'false'}\n")
        return
    github = GitHub(token)
    updated, changes, skipped = refresh(manifest, github)
    pending, compare_errors = untagged_changes(updated, github)
    full_report = report(changes, skipped) + "\n" + untagged_report(pending, compare_errors)
    args.report_dir.mkdir(parents=True, exist_ok=True)
    write_json(args.report_dir / "result.json", {
        "changes": changes,
        "skipped": skipped,
        "untagged": pending,
        "untagged_errors": compare_errors,
    })
    (args.report_dir / "report.md").write_text(full_report, encoding="utf-8")
    if changes:
        changed_ids = {change["id"] for change in changes}
        changed_manifest = deepcopy(updated)
        changed_manifest["templates"] = [
            template for template in updated["templates"] if template["id"] in changed_ids
        ]
        changed_manifest["totalTemplates"] = len(changed_manifest["templates"])
        write_json(args.report_dir / "changed-manifest.json", changed_manifest)
        write_json(args.manifest, updated)
    print(full_report, end="")
    output = os.environ.get("GITHUB_OUTPUT")
    if output:
        with open(output, "a", encoding="utf-8") as stream:
            stream.write(f"changed={'true' if changes else 'false'}\n")


if __name__ == "__main__":
    try:
        main()
    except (RuntimeError, ValueError, OSError, KeyError) as error:
        print(f"ERROR: {error}", file=sys.stderr)
        sys.exit(1)
