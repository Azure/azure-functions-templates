# Quickstart project-template packages

The manifest remains the discovery source of truth. `packageReference` is an
optional exact package nomination for Core Tools, not proof that a package was
published or approved for a particular installed stack. Only add it to the
production manifest after the exact package is available from its declared
feed and the CLI's independent artifact and stack association has been reviewed.

The manual packager in `eng/scripts/pack-quickstart.ps1` creates one local
project-template package from one manifest entry. It does not run source code,
restore dependencies, publish the package, change `manifest.json`, or authorize
acquisition. The output includes the `.nupkg`, source and package file
inventories, SHA-256 checksum, `release.json`, and `manifest.next.json`. The
manifest candidate is for review only. It must not be merged as an acquirable
entry before publication and independent CLI qualification.

## First release

Choose a source entry whose `gitRef` points to an annotated, GitHub-verified
signed tag. Resolve the tag to its commit and review the selected folder and
source license. Then supply the exact expected commit and a new package version:

```powershell
pwsh -NoProfile -File .\eng\scripts\pack-quickstart.ps1 `
  -TemplateId http-trigger-python-azd `
  -PackageVersion 1.0.0 `
  -ExpectedCommit <40-character-commit-sha> `
  -PackageSource https://api.nuget.org/v3/index.json `
  -OutputPath .\artifacts\quickstarts
```

The ID above is an existing manifest entry. The package ID and full engine
identity are derived once from that ID and retained for updates. The version,
feed URL, and expected source commit must be deliberately selected for the
release; the example version is illustrative and is not a published package.
Branch-backed entries are not packageable until their source is reviewed and
registered against an approved signed tag.

## Update

Use the same manifest ID, package ID, full template identity, and package feed.
Select the new signed source tag, provide its resolved commit, and increment the
package version:

```powershell
pwsh -NoProfile -File .\eng\scripts\pack-quickstart.ps1 `
  -TemplateId http-trigger-python-azd `
  -SourceRef refs/tags/<reviewed-new-tag> `
  -PackageVersion 1.0.1 `
  -ExpectedCommit <new-40-character-commit-sha> `
  -PackageSource https://api.nuget.org/v3/index.json `
  -OutputPath .\artifacts\quickstarts
```

Each package ID/version output directory is immutable. Repeating an identical
build is a no-op. Reusing a version with different package bytes or source
commit fails; issue a new version instead. The script does not push a package
or tag a source repository.

## Package and consumer boundary

The package is a single `type: project` template with copy-only project content.
It introduces only the engine metadata needed to enumerate and instantiate a
project. It does not add optional symbols, prompt for template parameters,
rewrite arbitrary source names, or run post-actions. A source folder that
already contains `.template.config` is rejected rather than silently rewritten.

The release record lists the exact source tag and commit, selected folder,
package coordinates, package SHA-256, and complete package file inventory. Its
template inventory identifies the generated config path, full identity, and
`project` type. The record is generated evidence, not approval. Core Tools
requires separate reviewed authority associating the exact artifact with a
compatible stack, feed, host/channel policy, and AdditionalProject role.
`packageReference` in the discovery manifest cannot supply that authority.

The packager accepts root folders written as either `.` or empty and supports
selected subfolders without including sibling projects. It rejects branch refs,
unsigned/lightweight tags, unsafe archive paths, symbolic links, case-insensitive
path collisions, oversized source files, and credential-like values in
`local.settings.json`. It does not execute archive contents. Source licensing
must be resolved before packaging. A selected-folder license is retained with
the project. If one is absent, the repository-root license and notice files are
carried as package legal files.

Run the offline checks, including a temporary-hive install and project creation
with the .NET template engine, with:

```powershell
pwsh -NoProfile -File .\eng\scripts\tests\test-pack-quickstart.ps1
```
