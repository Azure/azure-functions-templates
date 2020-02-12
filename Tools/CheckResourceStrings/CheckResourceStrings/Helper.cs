using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CheckResourceStrings
{
    class Helper
    {
        private static string ParseResourceName(string resourceName)
        {
            if (resourceName != null && resourceName.StartsWith("$") && resourceName.Length > 1)
            {
                return resourceName.Substring(1);
            }

            if (resourceName != null && resourceName.StartsWith("[variables(\'") && resourceName.EndsWith("')]"))
            {
                var variableName = resourceName.Split('\'')[1];
                return $"variables_{variableName}";
            }

            return null;
        }

        public static string GetLocalizedString(string stringResourceName, ResourceStringsObj resources, string locale)
        {
            if (stringResourceName == null)
            {
                return null;
            }

            var parsedResourceName = ParseResourceName(stringResourceName);
            var baseLocaleResources = locale == FunctionsConstants.EnglishUSLocale ? resources.EnglishResourceMap : resources.LanguageResourceMap;
            var fallbackLocaleResources = resources.EnglishResourceMap;

            if (baseLocaleResources.TryGetValue(parsedResourceName, out string localizedString) || fallbackLocaleResources.TryGetValue(parsedResourceName, out localizedString))
            {
                return localizedString;
            }
            else
            {
                return parsedResourceName;
            }
        }

        public static List<string> GetResourceStringNames(string path)
        {
            var resources = new List<string>();
            var fileContent = File.ReadAllText(path);
            using (var reader = new JsonTextReader(new StringReader(fileContent)))
            {
                while (reader.Read())
                {
                    var val = reader.Value?.ToString();
                    string parsedVal = ParseResourceName(val);
                    if (!string.IsNullOrEmpty(parsedVal))
                    {
                        resources.Add(parsedVal);
                    }
                }
            }
            return resources;
        }

        public static void CheckJsonFilePath(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException($"Invalid file path or file does not exist at {path}");
            }
        }
    }
}
