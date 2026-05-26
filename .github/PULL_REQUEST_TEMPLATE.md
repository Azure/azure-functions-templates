## Description

<!-- Brief description of changes -->

## Checklist

### When adding or updating templates

- [ ] `.nuspec` updated with new templates
- [ ] `Resources.resx` updated with template metadata
- [ ] Or: only updating an existing template

### When updating `Functions.Templates\Template-Manifest\manifest.json`

- [ ] Schema validation passes (`Test-Json` against `manifest.schema.json`)
- [ ] Ran `pwsh ./eng/scripts/validate-manifest-refs.ps1` locally and all refs resolve
- [ ] Priority tiers in `docs/priority-tiers.md` are consistent with manifest entries
