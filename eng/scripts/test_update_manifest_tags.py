import importlib.util
from pathlib import Path
import unittest
from unittest.mock import Mock


SPEC = importlib.util.spec_from_file_location(
    "update_manifest_tags", Path(__file__).with_name("update-manifest-tags.py")
)
updater = importlib.util.module_from_spec(SPEC)
SPEC.loader.exec_module(updater)


def manifest(*refs):
    return {
        "generatedAt": "2026-01-01T00:00:00Z",
        "version": "1.14.0",
        "totalTemplates": len(refs),
        "templates": [
            {
                "id": f"sample-{index}",
                "repositoryUrl": "https://github.com/example/samples",
                "gitRef": ref,
                "folderPath": "app",
                "priority": index,
            }
            for index, ref in enumerate(refs)
        ],
    }


class RefreshTests(unittest.TestCase):
    def github(self, tags, lightweight=(), unsigned=(), missing=()):
        github = Mock()
        github.tags.return_value = tags

        def get(endpoint, allow_missing=False):
            if "/git/ref/tags/" in endpoint:
                tag = endpoint.rsplit("/", 1)[1]
                return {"object": {"type": "commit" if tag in lightweight else "tag", "sha": tag}}
            if "/git/tags/" in endpoint:
                tag = endpoint.rsplit("/", 1)[1]
                return {
                    "verification": {"verified": tag not in unsigned},
                    "object": {"type": "commit"},
                }
            if "/contents/" in endpoint:
                return None if any(tag in endpoint for tag in missing) else [{"type": "file"}]
            raise AssertionError(f"Unexpected endpoint: {endpoint}")

        github.get.side_effect = get
        return github

    def test_numeric_order_shared_repository_and_metadata_preserved(self):
        original = manifest("refs/tags/v1.0.0", "refs/tags/v1.9.0")
        github = self.github(["v1.9.0", "v1.10.0", "v2.0.0-rc.1"])
        updated, changes, skipped = updater.refresh(original, github)
        self.assertEqual([c["new"] for c in changes], ["refs/tags/v1.10.0"] * 2)
        self.assertEqual(skipped, [])
        github.tags.assert_called_once_with("example/samples")
        self.assertRegex(updated["generatedAt"], r"^\d{4}-\d\d-\d\dT\d\d:\d\d:\d\dZ$")
        self.assertNotEqual(updated["generatedAt"], original["generatedAt"])
        expected = manifest("refs/tags/v1.10.0", "refs/tags/v1.10.0")
        expected["generatedAt"] = updated["generatedAt"]
        self.assertEqual(updated, expected)
        self.assertEqual(original, manifest("refs/tags/v1.0.0", "refs/tags/v1.9.0"))

    def test_skips_lightweight_unsigned_and_missing_folders(self):
        github = self.github(
            ["v5.0.0", "v4.0.0", "v3.0.0", "v2.0.0"],
            lightweight=["v5.0.0"],
            unsigned=["v4.0.0"],
            missing=["v3.0.0"],
        )
        updated, changes, skipped = updater.refresh(manifest("refs/tags/v1.0.0"), github)
        self.assertEqual(updated["templates"][0]["gitRef"], "refs/tags/v2.0.0")
        self.assertEqual(len(changes), 1)
        self.assertEqual(len(skipped), 3)

    def test_no_downgrades_branch_changes_or_timestamp_only_changes(self):
        original = manifest("refs/tags/v2.0.0", "refs/heads/main", "refs/tags/custom")
        updated, changes, skipped = updater.refresh(original, self.github(["v1.0.0", "v2.0.0"]))
        self.assertEqual(updated, original)
        self.assertEqual(changes, [])
        self.assertEqual(len(skipped), 2)

    def test_existing_empty_and_dot_root_folders_are_supported(self):
        for folder in ("", "."):
            with self.subTest(folder=folder):
                original = manifest("refs/tags/v1.0.0")
                original["templates"][0]["folderPath"] = folder
                github = self.github(["v2.0.0"])
                updated, changes, _ = updater.refresh(original, github)
                self.assertEqual(updated["templates"][0]["folderPath"], folder)
                self.assertEqual(len(changes), 1)
                github.get.assert_any_call(
                    "repos/example/samples/contents/?ref=refs%2Ftags%2Fv2.0.0",
                    allow_missing=True,
                )

    def test_api_failure_does_not_mutate_manifest(self):
        original = manifest("refs/tags/v1.0.0", "refs/tags/v1.0.0")
        original["templates"][1]["repositoryUrl"] = "https://github.com/example/other"
        github = self.github(["v2.0.0"])
        github.tags.side_effect = [["v2.0.0"], RuntimeError("HTTP 403")]
        before = updater.deepcopy(original)
        with self.assertRaisesRegex(RuntimeError, "HTTP 403"):
            updater.refresh(original, github)
        self.assertEqual(original, before)

    def test_paginates_tag_listing(self):
        github = updater.GitHub("test-token")
        github.get = Mock(side_effect=[
            [{"name": f"v1.0.{index}"} for index in range(100)],
            [{"name": "v2.0.0"}],
        ])
        self.assertEqual(len(github.tags("example/samples")), 101)
        self.assertEqual(
            github.get.call_args_list[-1].args,
            ("repos/example/samples/tags?per_page=100&page=2",),
        )

    def test_semver_policy(self):
        for tag in ["v1.2.3", "1.2.3", "v1.2.3+build.4"]:
            self.assertEqual(updater.version(tag), (1, 2, 3))
        for tag in ["v1.2.3-beta", "v01.2.3", "v1.2", "latest"]:
            self.assertIsNone(updater.version(tag))


class UntaggedChangeTests(unittest.TestCase):
    def github(self, ahead_by, files, fail=False):
        github = Mock()

        def get(endpoint):
            if endpoint == "repos/example/samples":
                return {"default_branch": "main"}
            if "/compare/" in endpoint:
                if fail:
                    raise RuntimeError("HTTP 404")
                return {
                    "ahead_by": ahead_by,
                    "files": [
                        file if isinstance(file, dict) else {"filename": file}
                        for file in files
                    ],
                }
            raise AssertionError(f"Unexpected endpoint: {endpoint}")

        github.get.side_effect = get
        return github

    def test_reports_changes_inside_sample_folder(self):
        pending, errors = updater.untagged_changes(
            manifest("refs/tags/v1.0.0"), self.github(2, ["app/main.py", "README.md"])
        )
        self.assertEqual(errors, [])
        self.assertEqual(pending[0]["folders"], ["app"])
        self.assertEqual(pending[0]["ahead_by"], 2)
        self.assertIn("please create signed tag", updater.untagged_report(pending, errors))

    def test_ignores_changes_outside_folder_and_branch_pins(self):
        original = manifest("refs/tags/v1.0.0", "refs/heads/main")
        pending, _ = updater.untagged_changes(
            original, self.github(3, ["other/file.py", "application/x.py"])
        )
        self.assertEqual(pending, [])

    def test_root_folder_reports_any_new_commit(self):
        original = manifest("refs/tags/v1.0.0")
        original["templates"][0]["folderPath"] = "."
        pending, _ = updater.untagged_changes(original, self.github(1, ["README.md"]))
        self.assertEqual(pending[0]["folders"], ["."])

    def test_compare_failure_is_reported_without_failing(self):
        pending, errors = updater.untagged_changes(
            manifest("refs/tags/v1.0.0"), self.github(0, [], fail=True)
        )
        self.assertEqual(pending, [])
        self.assertEqual(len(errors), 1)

    def test_network_io_failure_is_reported_without_failing(self):
        github = self.github(0, [])
        github.get.side_effect = OSError("connection reset")
        pending, errors = updater.untagged_changes(manifest("refs/tags/v1.0.0"), github)
        self.assertEqual(pending, [])
        self.assertIn("connection reset", errors[0])

    def test_renamed_file_matches_previous_sample_path(self):
        pending, errors = updater.untagged_changes(
            manifest("refs/tags/v1.0.0"),
            self.github(1, [{"filename": "archive/main.py", "previous_filename": "app/main.py"}]),
        )
        self.assertEqual(errors, [])
        self.assertEqual(pending[0]["folders"], ["app"])

    def test_sample_ids_are_limited_to_changed_folders(self):
        original = manifest("refs/tags/v1.0.0", "refs/tags/v1.0.0")
        original["templates"][1]["folderPath"] = "other"
        pending, _ = updater.untagged_changes(
            original, self.github(1, ["app/main.py"])
        )
        self.assertEqual(pending[0]["folders"], ["app"])
        self.assertEqual(pending[0]["ids"], ["sample-0"])

    def test_truncation_warning_is_preserved_when_folder_matches(self):
        files = ["app/main.py"] + [f"other/file-{index}.py" for index in range(299)]
        pending, _ = updater.untagged_changes(manifest("refs/tags/v1.0.0"), self.github(1, files))
        self.assertEqual(pending[0]["folders"], ["app"])
        self.assertTrue(pending[0]["truncated"])
        self.assertIn("file list truncated", updater.untagged_report(pending, []))


class PullRequestPlanTests(unittest.TestCase):
    def setUp(self):
        self.base = manifest("refs/tags/v1.0.0", "refs/tags/v1.0.0")
        self.head = manifest("refs/tags/v2.0.0", "refs/tags/v1.0.0")
        self.pull = {
            "number": 42,
            "html_url": "https://github.com/example/templates/pull/42",
            "user": {"login": "github-actions[bot]"},
            "base": {"sha": "latest-base"},
            "head": {
                "ref": "automation/manifest-tags-123-1",
                "sha": "old-head",
                "repo": {"full_name": "example/templates"},
            },
        }
        self.github = Mock()
        self.github.get.side_effect = self.get

    def get(self, endpoint):
        if "/pulls?" in endpoint:
            return [self.pull]
        if endpoint.endswith("/files?per_page=100"):
            return [{"filename": updater.MANIFEST_PATH}]
        if "/compare/" in endpoint:
            return {"merge_base_commit": {"sha": "original-base"}}
        if "/contents/" in endpoint:
            value = self.head if endpoint.endswith("ref=old-head") else self.base
            return {"content": updater.base64.b64encode(updater.json.dumps(value).encode()).decode()}
        raise AssertionError(f"Unexpected endpoint: {endpoint}")

    def plan(self, desired):
        changes = [
            {"id": template_id, "new": ref}
            for template_id, ref in updater.pin_changes(self.base, desired).items()
        ]
        return updater.plan_pull_request(
            desired, changes, self.github, "example/templates", "dev"
        )

    def test_identical_pins_keep_existing_pr_despite_new_timestamp(self):
        desired = updater.deepcopy(self.head)
        desired["generatedAt"] = "2026-11-19T08:17:00Z"
        plan = self.plan(desired)
        self.assertFalse(plan["create"])
        self.assertEqual(plan["replacement_url"], self.pull["html_url"])
        self.assertEqual(plan["superseded"], [])

    def test_additional_sample_update_supersedes_existing_pr(self):
        desired = manifest("refs/tags/v2.0.0", "refs/tags/v2.0.0")
        plan = self.plan(desired)
        self.assertTrue(plan["create"])
        self.assertEqual([pull["number"] for pull in plan["superseded"]], [42])
        self.assertEqual(plan["superseded"][0]["sha"], "old-head")

    def test_newer_version_of_same_sample_supersedes_existing_pr(self):
        plan = self.plan(manifest("refs/tags/v3.0.0", "refs/tags/v1.0.0"))
        self.assertTrue(plan["create"])
        self.assertEqual(len(plan["superseded"]), 1)

    def test_human_pr_is_never_closed(self):
        self.pull["user"]["login"] = "maintainer"
        plan = self.plan(self.head)
        self.assertTrue(plan["create"])
        self.assertEqual(plan["superseded"], [])

    def test_regression_does_not_close_or_replace_existing_pr(self):
        with self.assertRaisesRegex(ValueError, "not covered"):
            self.plan(manifest("refs/tags/v1.5.0", "refs/tags/v2.0.0"))

    def test_manual_metadata_edits_block_automatic_replacement(self):
        self.head["templates"][0]["priority"] = 99
        with self.assertRaisesRegex(ValueError, "non-pin changes"):
            self.plan(manifest("refs/tags/v3.0.0", "refs/tags/v2.0.0"))


if __name__ == "__main__":
    unittest.main()
