# Change List for Unused Resource Strings and Hardcoded Template Names Fix

## Issue
- [Issue #XXXX](link-to-issue): Unused strings in Resources.resx present and template names are hardcoded in metadata

## Problem Statement

1. There were many unreferenced entries in Resources.resx that were not being used
2. Template names were hardcoded in metadata.json files instead of using resource references like the description field does

## Solution Implemented

### 1. Enhanced CheckResourceStrings Tool

**Files Modified:**
- `Tools/CheckResourceStrings/CheckResourceStrings/CheckResourceStrings.csproj`
- `Tools/CheckResourceStrings/CheckResourceStrings/Program.cs`
- `Tools/CheckResourceStrings/CheckResourceStrings/Program.UnusedStrings.cs` (new file)

**Changes:**
- Updated target framework from .NET Core 3.1 to .NET 8.0 for better compatibility
- Added functionality to scan individual metadata.json files directly (not just compiled templates.json/bindings.json)
- Added functionality to parse Resources.resx file directly using XML
- Added new command-line parameter (third argument) to enable unused resource string detection
- Created `GetResourceStringNamesFromMetadataFiles()` method to scan all metadata.json files
- Created `GetResourceStringNamesFromResx()` method to parse Resources.resx
- Created `PrintUnusedResourceStrings()` method to report findings

**Usage:**
```bash
cd Tools/CheckResourceStrings/CheckResourceStrings
dotnet build -c Release
dotnet bin/Release/net8.0/CheckResourceStrings.dll <path-to-artifacts> false true
```

The third parameter (`true`) enables checking for unused resource strings.

### 2. Updated Resources.resx

**File Modified:**
- `Functions.Templates/Resources/Resources.resx`

**Changes:**
- Added 34 new resource string entries for template names:
  - AuthenticationEventTrigger_name
  - AuthenticationEventsTrigger_name
  - AzureBlobStorageTriggerUsingEventGrid_name
  - AzureBlobStorageTrigger_name
  - AzureCosmosDbTrigger_name
  - AzureEventGridCloudEventTrigger_name
  - AzureEventGridTrigger_name
  - AzureEventHubTrigger_name
  - AzureQueueStorageTrigger_name
  - AzureServiceBusQueueTrigger_name
  - AzureServiceBusTopicTrigger_name
  - CosmosDbTrigger_name
  - DaprPublishOutputBinding_name
  - DaprServiceInvocationTrigger_name
  - DaprTopicTrigger_name
  - DurableFunctionsEntity_name
  - DurableFunctionsEntityHttpStarter_name
  - DurableFunctionsHttpStarter_name
  - DurableFunctionsEntityClass_name
  - DurableFunctionsEntityFunction_name
  - HttpTriggerWithOpenapi_name
  - IotHubEventHub_name
  - KafkaOutput_name
  - KafkaTrigger_name
  - KustoInputBinding_name
  - KustoOutputBinding_name
  - MysqlInputBinding_name
  - MysqlOutputBinding_name
  - MysqlTrigger_name
  - RabbitmqTrigger_name
  - SqlInputBinding_name
  - SqlOutputBinding_name
  - SqlTrigger_name
  - SignalrNegotiateHttpTrigger_name

### 3. Updated All 201 metadata.json Files

**Files Modified:** All 201 template metadata.json files under `Functions.Templates/Templates/*/metadata.json`

**Pattern Changed:**
```json
// Before
{
    "name": "Azure Blob Storage trigger",
    "description": "$BlobTrigger_description"
}

// After
{
    "name": "$AzureBlobStorageTrigger_name",
    "description": "$BlobTrigger_description"
}
```

**Categories of Templates Updated:**
- Authentication Event Triggers
- Blob Triggers
- Cosmos DB Triggers
- Dapr Bindings and Triggers
- Durable Functions (Activities, Entities, Orchestrators, HTTP Starters)
- Event Grid Triggers
- Event Hub Triggers
- HTTP Triggers
- IoT Hub Triggers
- Kafka Triggers and Outputs
- Kusto Bindings
- MySQL Bindings and Triggers
- Queue Triggers
- RabbitMQ Triggers
- Service Bus Triggers
- SignalR Triggers
- SQL Bindings and Triggers
- Timer Triggers
- SendGrid Bindings

**Languages Affected:**
- C# (all versions: 3.x, 4.x, 5.x, 6.x)
- F#
- JavaScript
- TypeScript
- Python
- PowerShell
- Custom

## Impact

### Positive Changes

1. **Consistency**: All templates now use resource references for both `name` and `description` fields
2. **Localization**: Easier to translate template names to other languages
3. **Maintenance**: Centralized management of template names in Resources.resx
4. **Tracking**: Enhanced tool can now identify unused resource strings

### Metrics

- **Templates updated**: 201 (100% of all templates)
- **New resource strings added**: 34
- **Resource string usage in metadata.json**: Increased from 41 to 84
- **Files modified**: 203 total (201 metadata.json + 1 Resources.resx + 1 tool enhancement)

### Unused Resource Strings

- **Before fix**: 645 unused resource strings
- **After fix**: 636 unused resource strings
- **Net change**: 9 fewer unused strings (34 new added - 43 newly used)

Note: The remaining 636 unused strings are mostly related to bindings metadata, UI elements, and deprecated features. Further cleanup can be done in a separate effort.

## Testing Recommendations

1. **UI Testing**: Test template creation in all supported tools:
   - Azure Portal
   - Visual Studio 2019/2022
   - Visual Studio Code
   - Azure Functions Core Tools

2. **Localization Testing**: Verify template names display correctly in:
   - English (en-US)
   - All supported locales (cs-CZ, de-DE, es-ES, fr-FR, etc.)

3. **Build Testing**: Ensure the build process correctly processes Resources.resx

4. **Runtime Testing**: Verify templates function correctly after deployment

## Migration Guide for Future Template Additions

When adding new templates, follow this pattern:

1. Add a resource string to Resources.resx:
```xml
<data name="MyNewTrigger_name" xml:space="preserve">
  <value>My New Trigger</value>
</data>
```

2. Reference it in metadata.json:
```json
{
  "name": "$MyNewTrigger_name",
  "description": "$MyNewTrigger_description"
}
```

3. Verify with the CheckResourceStrings tool before submitting PR

## Documentation

- **Comprehensive Analysis**: [UnusedResourceStringsAnalysis.md](./UnusedResourceStringsAnalysis.md)
- **Tool Documentation**: See README in `Tools/CheckResourceStrings/`

## Rollback Instructions

If issues are discovered and rollback is needed:

1. Revert the commit that updated metadata.json files
2. Revert the commit that added new resource strings
3. Tool enhancements can remain as they're backwards compatible

## Future Work

1. Update all localized resource files (Resources.*.json) with translations
2. Review and potentially remove the 636 still-unused resource strings
3. Integrate CheckResourceStrings tool into CI/CD pipeline
4. Consider standardizing naming conventions (e.g., "Azure Blob Storage trigger" vs "Blob trigger")
