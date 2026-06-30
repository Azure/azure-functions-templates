---
name: add-manifest-sample
description: >-
  Add a new Azure Functions sample/quickstart to the Template Manifest. Use when
  asked to "add a sample", "add a template", "register a quickstart", or
  "add a connector/MCP/trigger sample" to
  Functions.Templates/Template-Manifest/manifest.json. Covers required fields,
  the short/long description pattern, category rules, priority assignment,
  metadata bumps (count/version/generatedAt), and keeping docs/priority-tiers.md
  in sync.
---

# Add a new sample to the Template Manifest

This skill explains how to register a new template in the Azure Functions
Template Manifest so it appears in the VS Code Azure Functions extension and
other consumers.

## Who consumes the manifest

Changes to `manifest.json` are surfaced by multiple downstream consumers, so keep
entries accurate and validated:

- **Azure Functions VS Code Extension** — shows templates (and their
  `categories`) in the create-project / create-sample experience.
- **Azure Functions CLI (Core Tools) v5** — quickstart command uses the manifest to list and
  scaffold samples.
- **Azure MCP tool** — exposes the manifest's templates to MCP clients/agents in Azure Functions Tools.
- **Azure skills** — Copilot/agent skills that read the manifest to recommend or
  generate Functions samples (entry point Azure MCP Tool)

## Files you will touch

| File | Purpose |
|---|---|
| `Functions.Templates/Template-Manifest/manifest.json` | Source of truth. Add the template object + bump metadata. |
| `Functions.Templates/Template-Manifest/schemas/manifest.schema.json` | JSON Schema. Only edit to add a **new enum value** (e.g. a new `resource`). Note: `categories` are **not** a schema enum — they are free-form kebab-case (pattern-validated), so adding a category needs **no** schema change. |
| `Functions.Templates/Template-Manifest/docs/priority-tiers.md` | Human-readable priority/coverage doc. Keep in sync with the manifest. |

The manifest file is written as canonical `json.dumps(indent=2, ensure_ascii=False)`
output with a trailing newline. Preserve that formatting (2-space indent, no
trailing whitespace).

## Step 1 — Inspect the source repository first

Never guess what a sample does. Inspect the actual repo at the exact tag you will
pin (`gitRef`) and confirm:

- The folder (`folderPath`) and tag (`gitRef`) exist.
- The real trigger/function names.
- Whether it actually uses a binding you plan to mention. For example, only claim
  "Blob output" if the code really has a Blob output binding — sibling samples in
  different languages often differ (e.g. C# may use `BlobOutput` while the
  TypeScript/Python ports only log the payload).

```powershell
gh api "repos/<owner>/<repo>/contents/<folderPath>?ref=refs/tags/<tag>" --jq '.[] | "\(.type)\t\(.name)"'
```

## Step 2 — Add the template object

Insert the template object adjacent to its peer entries (same `resource` value)
in the `templates` array, maintaining priority order within that group. If no
peers exist, append to the end of the array. Mirror the existing entry for the
same family/resource so the new one is consistent.

### Mandatory fields for a new sample

A new template **must** include **all** of the following.

**Enforced by the schema:**
`id`, `displayName`, `shortDescription`, `longDescription`, `language`, `bindingType`, `resource`,
`iac`, `priority`, `categories`, `repositoryUrl`, `folderPath`, `gitRef`.

Other fields (`whatsIncluded`, `author`, `isHighlighted`) are
optional but recommended where applicable. **`tags` is deprecated — do not add it.**

### Field rules

- **`id`** — kebab-case, unique (matches the schema `id` pattern). Convention:
  `<resource-or-feature>-<binding>-<language>` (e.g. `office365-connector-trigger-python`).
  Before adding the entry, search `templates` for a matching `id`. If a collision
  is found, inform the user and suggest an alternative `id` (for example, append
  a version suffix or disambiguating term). Never silently overwrite or duplicate
  an `id`.
- **`displayName`** — `"<Friendly Name> (<Lang> + AZD + Bicep)"`.
- **`language`** — must be one of the schema `language` enum values.
- **`bindingType`** — must be one of the schema `bindingType` enum values.
- **`resource`** — must be one of the schema `resource` enum values. Add a new
  enum value to `manifest.schema.json` **only** if none fits, and reuse it
  consistently.
- **`iac`** — must be one of the schema `iac` enum values.
- The same enum-extension rule applies to `language`, `bindingType`, and `iac`:
  add a new enum value in `manifest.schema.json` only when no existing value fits,
  then reuse it consistently across related samples. When any enum is extended,
  bump manifest `version` with a minor increment.
- **`repositoryUrl`** — must satisfy the schema pattern (GitHub HTTPS URL).
- **`folderPath`** — path within the repo (`.` for root).
- **`gitRef`** — **mandatory for new samples.** Always pin a signed release tag,
  e.g. `refs/tags/v1.0.0`. Never leave it unset and never point a new sample at a
  moving branch (`refs/heads/main`) — that makes the template non-reproducible.
  Verify the tag exists before committing. **Whenever the sample repo ships a new
  release, ask the user to update `gitRef` to the new tag** (and re-verify the
  pinned `folderPath`/contents still match) so the manifest tracks the intended
  release.
  If the `gitRef` tag does not exist in the target repository, do not proceed
  with adding the template. Inform the user that the tag was not found, provide
  the exact `gh api` command used to verify, and ask them to confirm the tag name
  or create the release first.

The allowed values, patterns, and limits for every field are defined in
`schemas/manifest.schema.json` — read them there rather than relying on a copy.
A quick way to print the enums:

```powershell
cd Functions.Templates\Template-Manifest
@'
import json
p = json.load(open(r"schemas\manifest.schema.json", encoding="utf-8"))["$defs"]["template"]["properties"]
for f in ("language", "bindingType", "resource", "iac"):
    print(f, "->", p[f]["enum"])
'@ | python -
```

### Categories (match existing — shown in VS Code)

`categories` are displayed/filtered in the VS Code extension, so **only use
categories that already exist in the manifest** unless you have a deliberate reason
to introduce a new one. Derive the current set from the manifest instead of
hardcoding it:

```powershell
cd Functions.Templates\Template-Manifest
@'
import json
d = json.load(open("manifest.json", encoding="utf-8"))
print(sorted({c for t in d["templates"] for c in t["categories"]}))
'@ | python -
```

Most starter samples include `starters` plus a domain category
(e.g. `["starters", "connectors", "event-processing"]`). Values are kebab-case.

### `tags` are deprecated

**Do not add a `tags` array to new entries.** It is deprecated and not used by
consumers. (Older entries may still contain it; leave those as-is unless asked.)

### Description pattern

Match the established pattern used across the manifest:

- **`shortDescription`** (respect the schema's `maxLength`): a concise capability
  summary that ends with **`deployed via azd`** for azd-based templates.
  Pattern: `"<Service> <scope> <mechanism>, deployed via azd"`.
  Example: `"Office 365 Outlook email and calendar connector triggers with Blob output, deployed via azd"`.
  If the sample is not azd-based (for example, `iac` is `none` or deployment is
  manual), omit the `deployed via azd` suffix. End with the real deployment
  mechanism (for example, `deployed via ARM template`) or with the capability
  summary when no deployment mechanism is represented.

- **`longDescription`**: start with the imperative
  **`Build and deploy …`**, then **explicitly name the trigger(s) and binding(s)
  the sample demonstrates** — e.g. the specific trigger type(s)
  (`ConnectorTrigger`, `McpToolTrigger`, `CosmosDBTrigger`, …) and any input/output
  bindings (`BlobInput`/`BlobOutput`, etc.). Enumerate `(1) … (2) …` when there are
  multiple functions/tools, naming each one. Only list a binding if the sample's
  code actually uses it (verify per language — siblings often differ). Then name
  the SDK/extension, give the hosting/auth summary, and **end with
  `Deployed with azd.`**
  For non-azd samples, omit the `Deployed with azd.` closing and end with the
  deployment approach actually used or a neutral capability summary.

- **`whatsIncluded`**: a short list of concrete artifacts (functions, bindings,
  infra, azd config). Keep claims accurate to the real sample.

### Example object

```json
{
  "id": "office365-connector-trigger-python",
  "displayName": "Office 365 Outlook Connector Triggers (Python + AZD + Bicep)",
  "shortDescription": "Office 365 Outlook email and calendar connector triggers, deployed via azd",
  "longDescription": "Build and deploy an Office 365 Outlook connector app on Azure Functions in Python. This app provides five connector trigger functions ... Deployed with azd.",
  "language": "Python",
  "bindingType": "trigger",
  "resource": "connector",
  "iac": "bicep",
  "priority": 80,
  "categories": ["starters", "connectors", "event-processing"],
  "author": "Azure Functions Team",
  "repositoryUrl": "https://github.com/Azure-Samples/functions-connectors-python",
  "folderPath": "office365App",
  "gitRef": "refs/tags/v1.0.0",
  "whatsIncluded": ["..."]
}
```

`isHighlighted: true` is optional and only for samples that should be
featured/highlighted in the consuming UI (e.g. VS Code).

## Step 3 — Choose a priority

`priority` is the primary sort key (lower = shown first), an integer within the
range defined by the schema's `priority` bounds. It is organized in blocks (see
`docs/priority-tiers.md`):

- 5-slot blocks per Azure resource: `+0` trigger, `+1` input, `+2` output,
  `+3` variant, `+4` stub.
- 10-slot block for MCP (P50–59).
- Language samples that share a sub-type share the same priority and are sorted by
  language order: **.NET → Python → TypeScript → JavaScript → Java → PowerShell**.

Use this lookup style to avoid inferring block math from memory. Build/update this
table from `docs/priority-tiers.md` before assigning a new value:

| Resource | Block Start | trigger | input | output | variant |
|---|---:|---:|---:|---:|---:|
| mcp | 50 | 50 | 51 | 52 | 53 |
| connector | 80 | 80 | 81 | 82 | 83 |

If the target resource/block is unclear or overlaps with existing assignments,
stop and confirm the intended block with the user before setting `priority`.

Reuse the priority of the matching peer template (e.g. all Office 365 connector
samples = P80, all SharePoint = P81). Use the next free slot/block for a brand-new
category.

## Step 4 — Bump manifest metadata

In `manifest.json`, update the top-level fields:

- **`totalTemplates`** — must equal the number of objects in `templates`.
- **`version`** — bump semver (minor bump for new templates / a new `resource`
  enum value; patch for description-only fixes).
  Use these exact rules: bump the **minor** version (for example, `1.2.0` →
  `1.3.0`) if the PR adds one or more new templates or introduces a new enum
  value (`resource`, `language`, `bindingType`, or `iac`). Bump the **patch**
  version (for example, `1.2.0` → `1.2.1`) if the PR only fixes descriptions or
  metadata without adding/removing templates or extending enums. If both occur in
  one PR, use the higher bump (minor).
- **`generatedAt`** — current UTC time in `YYYY-MM-DDTHH:MM:SSZ`.

## Step 5 — Update `docs/priority-tiers.md`

Keep the doc in sync with the manifest:

- Update the header **Total templates** and **Manifest version**.
- Add the new row(s) to the **Full template listing** (use `*new*` in the *Was*
  column), in priority → language order.
- Update the **Priority block map** and **Coverage matrix** if you added a
  sub-type/category, and the **Summary** counts and **Total** (must match
  `totalTemplates`).
- If samples are removed (e.g. archived repos), delete their rows and note the
  removal in the block map / coverage matrix (e.g. `*(removed — repos archived)*`).

## Step 6 — Validate

Always validate JSON parses and conforms to the schema, and that the counts match
(install the validator first if needed: `pip install jsonschema`):

```powershell
cd Functions.Templates\Template-Manifest
@'
import json, jsonschema
d = json.load(open("manifest.json", encoding="utf-8"))
s = json.load(open(r"schemas\manifest.schema.json", encoding="utf-8"))
jsonschema.validate(d, s)
assert d["totalTemplates"] == len(d["templates"]), "totalTemplates mismatch"
print("OK:", len(d["templates"]), "templates, version", d["version"])
'@ | python -
```

Also confirm a full-file round-trip produces no unintended formatting drift before
and after large edits:

```python
orig = open("manifest.json", encoding="utf-8").read()
import json
assert orig == json.dumps(json.loads(orig), indent=2, ensure_ascii=False) + "\n"
```

### Validate template references on GitHub

Run the repo's reference validator, which checks that every template's
`repositoryUrl`, `gitRef`, and `folderPath` actually resolve on GitHub (and warns
when a `gitRef` is a branch instead of a signed tag). Requires an authenticated
`gh` CLI:

```powershell
pwsh -NoProfile -File eng\scripts\validate-manifest-refs.ps1
```

The script validates the whole manifest, so pre-existing `refs/heads/*` branch
entries may already emit warnings. **For the sample(s) you added or changed, the
output must show `OK` with no `WARN`/`ERROR` lines** — i.e. the repo, tag, and
folder resolve and `gitRef` is a signed tag (`refs/tags/*`). Fix any error and
re-run until your entries are clean before committing.

## Checklist

- [ ] Verified repo, `folderPath`, and `gitRef` (tag) exist; claims match real code.
- [ ] `gitRef` set to a pinned release tag (mandatory; not a moving branch).
- [ ] Template object added with all mandatory fields (incl. `gitRef`); `id` unique.
- [ ] `displayName` follows `"<Friendly Name> (<Lang> + AZD + Bicep)"`.
- [ ] `categories` use only existing values; **no `tags`** on new entries.
- [ ] `shortDescription` follows deployment mode rules: for azd templates it ends with
  `deployed via azd`; for non-azd templates it omits that suffix and uses the
  actual deployment mechanism (or a capability-only ending).
- [ ] `longDescription` starts with `Build and deploy …`, **explicitly names the
  trigger(s)/binding(s) demonstrated**; for azd templates it ends with
  `Deployed with azd.`, and for non-azd templates it omits that closing and
  uses the actual deployment approach (or a neutral capability summary).
- [ ] `priority` matches peers / correct block.
- [ ] `totalTemplates`, `version`, `generatedAt` updated.
- [ ] `docs/priority-tiers.md` listing, block map, coverage matrix, and totals updated.
- [ ] JSON parses, schema validates, counts match; no formatting drift (round-trip).
- [ ] `eng\scripts\validate-manifest-refs.ps1` shows `OK` (no `WARN`/`ERROR`) for
      the sample(s) you touched.
