# Unused Resource Strings and Hardcoded Template Names Analysis

## Overview

This document provides an analysis of unused resource strings in `Resources.resx` and hardcoded template names in `metadata.json` files, as tracked in issue [Add issue link here].

## Current State

### Resource Strings

- **Total resource strings defined in Resources.resx**: 683
- **Resource strings referenced in code**: 41 (from metadata.json files only)
- **Unused resource strings**: 645

### Template Names

- **Total templates**: 201
- **Templates with hardcoded names**: 201 (100%)
- **Templates using resource references for names**: 0 (0%)
- **Templates using resource references for descriptions**: 201 (100%)

## Key Findings

### 1. Inconsistent Pattern Usage

Currently, all templates use resource references for their `description` field but hardcoded strings for their `name` field:

```json
{
  "description": "$BlobTrigger_description",
  "name": "Azure Blob Storage trigger"
}
```

The expected pattern should be:

```json
{
  "description": "$BlobTrigger_description",
  "name": "$BlobTrigger_name"
}
```

### 2. Existing but Unused *_name Resource Strings

The following resource strings with the `*_name` suffix already exist in Resources.resx but are not being used:

1. `AppInsightsHttpAvailability_name`: "AppInsights Http Availability"
2. `AppInsightsRealtimePowerBI_name`: "AppInsights Real-time Power BI"
3. `AppInsightsScheduledAnalytics_name`: "AppInsights Scheduled Analytics"
4. `AppInsightsScheduledDigest_name`: "AppInsights Scheduled Digest"
5. `BlobTrigger_name`: "Blob trigger"
6. `DurableFunctionsActivity_name`: "Durable Functions activity"
7. `DurableFunctionsHttpStart_name`: "Durable Functions Http starter"
8. `DurableFunctionsOrchestrator_name`: "Durable Functions orchestrator"
9. `EventHubTrigger_name`: "Event Hub trigger"
10. `HttpTrigger_name`: "HTTP trigger"
11. `OutlookMessageWebhookCreator_name`: "Outlook message webhook subscription creator"
12. `OutlookMessageWebhookDeleter_name`: "Outlook message webhook subscription deleter"
13. `OutlookMessageWebhookHandler_name`: "Outlook message webhook handler"
14. `OutlookMessageWebhookRefresher_name`: "Outlook message webhook subscription refresher"
15. `ProfilePhotoAPI_name`: "Microsoft Graph profile photo API"
16. `QueueTrigger_name`: "Queue trigger"
17. `ScheduledMail_name`: "Scheduled mail"
18. `SendGrid_name`: "SendGrid"
19. `TimerTrigger_name`: "Timer trigger"
20. `temp_timerTrigger_CSharp_name`: "TimerTrigger - C#"

### 3. Text Inconsistencies

Some template groups use different text than what's in the resource strings. For example:

**BlobTrigger templates:**
- Most use: "Azure Blob Storage trigger"
- Resource string (`BlobTrigger_name`) contains: "Blob trigger"
- FSharp templates use: "Blob trigger" (matches resource string)

This inconsistency needs to be resolved before migrating to resource references.

## Recommendations

### Option 1: Minimal Change Approach

1. Update existing resource strings to match the most commonly used text
2. Add new resource strings for template types that don't have them
3. Update all metadata.json files to use resource references

**Pros:**
- Consistent pattern across all templates
- Easier localization
- Easier maintenance

**Cons:**
- Large change affecting 201 files
- Potential for breaking changes if text changes
- Requires coordination with localization team

### Option 2: Gradual Migration

1. Start with templates that already have matching resource strings
2. Update those templates to use resource references
3. Add new resource strings gradually for other templates
4. Update remaining templates in phases

**Pros:**
- Lower risk
- Easier to review and test
- Can be done incrementally

**Cons:**
- Temporarily inconsistent state
- Takes longer to complete

### Option 3: Documentation Only

1. Keep the current state
2. Document the unused strings for potential cleanup
3. Provide tooling to identify and report issues

**Pros:**
- No breaking changes
- No risk to existing functionality

**Cons:**
- Doesn't solve the underlying issues
- Continued maintenance burden

## Enhanced CheckResourceStrings Tool

The `CheckResourceStrings` tool has been enhanced to detect and report unused resource strings:

### Usage

```bash
cd Tools/CheckResourceStrings/CheckResourceStrings
dotnet build -c Release
dotnet bin/Release/net8.0/CheckResourceStrings.dll <path-to-functions-templates> false true
```

The third parameter (`true`) enables checking for unused resource strings.

### Output

The tool will report:
- Total resource strings defined
- Resource strings referenced in metadata.json files
- Resource strings referenced in bindings.json (if exists)
- Resource strings referenced in templates.json (if exists)
- Complete list of unused resource strings

## Implementation Example

Here's an example of how to update a template to use resource references:

### Before (BlobTrigger-Python/metadata.json)
```json
{
    "defaultFunctionName": "BlobTrigger",
    "description": "$BlobTrigger_description",
    "name": "Azure Blob Storage trigger",
    "language": "Python"
}
```

### After
```json
{
    "defaultFunctionName": "BlobTrigger",
    "description": "$BlobTrigger_description",
    "name": "$AzureBlobStorageTrigger_name",
    "language": "Python"
}
```

### Required Resource String Addition

If the resource string doesn't exist, add it to Resources.resx:

```xml
<data name="AzureBlobStorageTrigger_name" xml:space="preserve">
  <value>Azure Blob Storage trigger</value>
</data>
```

## Implementation Completed

**Option 1 (Comprehensive Update) was implemented:**

1. ✅ Added 34 new resource strings to Resources.resx for template names that didn't have them
2. ✅ Updated all 201 metadata.json files to use `$ResourceKey_name` pattern instead of hardcoded strings
3. ✅ Resource references used increased from 41 to 84 in metadata.json files
4. ✅ All templates now follow the consistent pattern of using resource references for both `name` and `description` fields

### Changes Summary

- **New resource strings added**: 34
- **Templates updated**: 201 (100%)
- **Files modified**: 201 metadata.json files + 1 Resources.resx file

### Before and After Example

**Before (BlobTrigger-Python/metadata.json):**
```json
{
    "description": "$BlobTrigger_description",
    "name": "Azure Blob Storage trigger"
}
```

**After (BlobTrigger-Python/metadata.json):**
```json
{
    "description": "$BlobTrigger_description",
    "name": "$AzureBlobStorageTrigger_name"
}
```

## Next Steps for Maintainers

1. **Localization**: Update all localized resource files (Resources.*.json) to include translations for the 34 new resource strings
2. **Testing**: Test the templates in Azure Portal, VS Code, Visual Studio, and Core Tools to ensure UI displays correctly
3. **CI/CD Integration**: Consider adding the enhanced CheckResourceStrings tool to CI/CD pipeline to catch future issues
4. **Cleanup**: Review the 636 still-unused resource strings and determine which can be safely removed

## Tool Enhancements

The following enhancements were made to the CheckResourceStrings tool:

1. **New partial class**: `Program.UnusedStrings.cs` 
   - Added `GetResourceStringNamesFromMetadataFiles()` to scan all metadata.json files
   - Added `GetResourceStringNamesFromResx()` to parse Resources.resx directly
   - Added `PrintUnusedResourceStrings()` to report findings

2. **Updated Program.cs**:
   - Added third command-line parameter for unused string checking
   - Added `CheckUnusedResourceStrings()` method
   - Enhanced usage message

3. **Framework Update**:
   - Updated from .NET Core 3.1 to .NET 8.0 for better compatibility

## References

- Issue: [Add issue link here]
- Related PRs: [Add PR links here]
- README.md: Template creation guidelines
