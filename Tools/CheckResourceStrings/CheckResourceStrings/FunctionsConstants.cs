using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CheckResourceStrings
{
    public static class FunctionsConstants
    {
        public const string FunctionsCacheSectionName = "functions";
        public const string FunctionCacheSectionName = "function";
        public const string SystemCacheSectionName = "system";
        public const string HostCacheSectionName = "host";
        public const string SecretsCacheSectionName = "secrets";
        public const string MasterKeyCacheName = "master";
        public const string BindingsSectionName = "bindings";
        public const string BindingTypeConfigurationName = "type";
        public const string HttpTriggerBindingTypeName = "httpTrigger";
        public const string HttpTriggerRouteName = "route";
        public const string KeysSectionName = "keys";

        public const string BlobSecretStorageType = "blob";
        public const string FilesSecretStorageType = "files";
        public const string KeyVaultSecretStorageType = "keyvault";

        public const string ARMCacheFeatureFlag = "FunctionsV2ARMCacheEnabled";

        public const string FunctionDisabledConfigPropertyName = "disabled";
        public const string FunctionDisabledValue = "disabled";
        public const string FunctionEnabledValue = "enabled";

        public const string DefaultExtensionBundleSourceUri = "https://functionscdn.azureedge.net/public";
        public const string BindingMetadataFilePath = "bindings/bindings.json";
        public const string TemplatesCodeFilePath = "templates/templates.json";
        public const string ResourcesFilePathFormat = "resources/Resources.{0}.json";
        public const string ExtensionBundleDirectory = "ExtensionBundles";
        public const string EnglishUSLocale = "en-US";

        public enum TemplateDataSource
        {
            None,
            HostDefault,
            ExtensionBundle
        }

        public static Dictionary<string, string> RuntimeLanguageMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"dotnet", "c#"},
            {"node", "javascript"},
            {"powershell", "powershell"},
            {"python", "python"}
        };

        public static ICollection<string> Locales = new Collection<string>()
        {
            "cs-CZ",
            "de-DE",
            "es-ES",
            "fr-FR",
            "hu-HU",
            "it-IT",
            "ja-JP",
            "ko-KR",
            "nl-NL",
            "pl-PL",
            "pt-BR",
            "pt-PT",
            "ru-RU",
            "sv-SE",
            "tr-TR",
            "zh-CN",
            "zh-TW"
        };

        public enum TemplateDataFilter
        {
            FilterByName,
            None
        }

        public enum TemplateDataFormat
        {
            Compact,
            Detailed
        }

        public enum TemplateContentType
        {
            TemplateCode,
            BindingMetadata,
            LocalizedStrings
        }
    }
}
