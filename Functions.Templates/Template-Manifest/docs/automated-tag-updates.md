# Automated sample tag updates

The **Update manifest sample tags** GitHub Actions workflow checks every distinct
`repositoryUrl` in `manifest.json` daily at **08:17 UTC**. It also supports manual
dispatch. It always reads and targets the repository's actual default branch
(`dev` today). Scheduled workflows must be present on that default branch before
they can run; GitHub may delay scheduled runs.

The updater paginates repository tags and selects the highest stable SemVer
version newer than each entry's current pin, accepting an optional `v` prefix
and build metadata. Prerelease and non-SemVer tags are not candidates. Major
version updates are included for human review, never merged automatically.

Candidates must be annotated tags whose signatures GitHub reports as verified,
point directly to commits, and contain the entry's `folderPath` as a directory.
The updater skips ineligible candidates and tries the next highest version.
Unsigned/lightweight tags are never an automatic exception, even if an existing
pin was approved manually. Branch and non-stable pins are left unchanged and
reported for manual review.

Only changed `gitRef` values and `generatedAt` are written. The timestamp uses
UTC, for example `2026-11-17T21:56:11Z`, and changes only when at least one pin
changes. Manifest version, template count, ordering, priorities and other
metadata are preserved.

## Validation and pull requests

The workflow runs focused updater tests, validates the complete updated manifest
against its schema with PowerShell `Test-Json`, and runs
`eng/scripts/validate-manifest-refs.ps1` against a temporary manifest containing
only changed entries. This avoids blocking updates on pre-existing signature
exceptions in unchanged entries; the PR explicitly reports that scope.

One consolidated PR contains the existing repository PR template, validation
results, old/new pins and skipped candidates. Discovery reports also appear in
the Actions run summary, including no-change runs. API, authentication and rate
limit errors fail the run before the manifest is written rather than being
treated as "no updates."

If an existing automation PR proposes exactly the same pins, it is retained
without a duplicate or timestamp-only replacement. When additional or newer
updates arrive, the workflow first creates a consolidated replacement PR, then
closes superseded PRs with a comment linking to the replacement. If creation
fails, older PRs stay open. Each new PR uses a unique branch; if creation fails
after pushing, that branch remains available for manual recovery.

Automatic closure is limited to `github-actions[bot]` PRs from this repository's
`automation/manifest-tags-` branches targeting the default branch. The workflow
checks that they only modify manifest pins and the timestamp, and that the
replacement covers every prior pin with an equal or newer version. Human PRs
are not closed. Unexpected edits, removed samples or regressions stop publishing
for manual review. A head-SHA check immediately before closure also catches
concurrent commits; a failure can leave both PRs open rather than lose changes.

## Repository setup

No AI credentials, new source-repository tags or upstream write access are
needed. Reads use the workflow's `GITHUB_TOKEN` against public sample repositories;
inaccessible/private repositories cause an explicit failure.

Enable Actions for this repository and allow **GitHub Actions to create and
approve pull requests** in repository Actions settings (subject to organization
policy). The job requests `contents: write` and `pull-requests: write` solely to
push its update branch and open a PR. It never approves or merges PRs.

PRs opened with `GITHUB_TOKEN` do not trigger ordinary `push`/`pull_request`
GitHub Actions workflows. This workflow performs its own validation; if additional
Actions-based PR checks must run, separately configure an approved GitHub App
token and its minimum permissions before adopting that authentication path.
This does not change any Azure Pipelines policies.

## Local checks

Run the offline updater tests from the repository root:

```sh
python3 -m unittest discover -s eng/scripts -p 'test_update_manifest_tags.py' -v
```

For a manual preview, copy the manifest to a temporary directory, authenticate
`gh` with read access to the sample repositories, and run:

```sh
preview_dir="$(mktemp -d)"
cp Functions.Templates/Template-Manifest/manifest.json "$preview_dir/manifest.json"
GH_TOKEN="$(gh auth token)" python3 eng/scripts/update-manifest-tags.py \
  --manifest "$preview_dir/manifest.json" --report-dir "$preview_dir/report"
```

Inspect the copied manifest and `report/report.md`. This command writes only to
the named temporary directory; it does not push a branch or create a PR.
