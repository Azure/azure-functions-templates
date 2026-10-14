using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CheckResourceStrings
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Path to root of the template artifacts missing");
                Console.WriteLine("Usage: CheckResourceStrings <path-to-artifacts> [checkAllLocales] [checkUnused]");
                return;
            }

            bool checkUnused = args.Length > 2 && bool.TryParse(args[2], out bool unused) && unused;

            if (checkUnused)
            {
                Console.WriteLine("Checking for unused resource strings...\n");
                CheckUnusedResourceStrings(args[0]);
                Console.WriteLine("\n" + "=".PadRight(80, '=') + "\n");
            }

            string templateJsonLocation = Path.Combine(args[0], "templates", "templates.json");
            Helper.CheckJsonFilePath(templateJsonLocation);

            string bindingJsonLocation = Path.Combine(args[0], "bindings", "bindings.json");
            Helper.CheckJsonFilePath(bindingJsonLocation);

            var resoucesStringNames = Helper.GetResourceStringNames(templateJsonLocation);
            resoucesStringNames.AddRange(Helper.GetResourceStringNames(bindingJsonLocation));

            string resourcesLocation = Path.Combine(args[0], "resources");

            string defaultResourceFilePath = Path.Combine(resourcesLocation, $"Resources.json");
            Helper.CheckJsonFilePath(defaultResourceFilePath);
            var content = File.ReadAllText(defaultResourceFilePath);
            var resourceStrings = JsonConvert.DeserializeObject<ResourceStringsObj>(content);
            PrintMissingResourceStrings(resourceStrings.EnglishResourceMap, resoucesStringNames, defaultResourceFilePath);

            if (args.Length > 1 && bool.TryParse(args[1], out bool checkAllLocales) && checkAllLocales)
            {
                defaultResourceFilePath = Path.Combine(resourcesLocation, $"Resources.en-US.json");
                Helper.CheckJsonFilePath(defaultResourceFilePath);
                content = File.ReadAllText(defaultResourceFilePath);
                resourceStrings = JsonConvert.DeserializeObject<ResourceStringsObj>(content);
                PrintMissingResourceStrings(resourceStrings.EnglishResourceMap, resoucesStringNames, defaultResourceFilePath);

                foreach (var locale in FunctionsConstants.Locales)
                {
                    string resourceFilePath = Path.Combine(resourcesLocation, $"Resources.{locale}.json");
                    Helper.CheckJsonFilePath(resourceFilePath);
                    var fileContent = File.ReadAllText(resourceFilePath);
                    var bundleresourceStrings = JsonConvert.DeserializeObject<ResourceStringsObj>(fileContent);
                    PrintMissingResourceStrings(bundleresourceStrings.LanguageResourceMap, resoucesStringNames, resourceFilePath);
                }
            }

            Console.ReadLine();
        }

        public static void CheckUnusedResourceStrings(string artifactsRoot)
        {
            // Get the path to the Templates directory
            string templatesRoot = Path.Combine(artifactsRoot, "..", "Functions.Templates", "Templates");
            if (!Directory.Exists(templatesRoot))
            {
                // Try alternative path (if running from different location)
                templatesRoot = Path.Combine(artifactsRoot, "Templates");
                if (!Directory.Exists(templatesRoot))
                {
                    Console.WriteLine($"Templates directory not found at: {templatesRoot}");
                    return;
                }
            }

            // Get the path to Resources.resx
            string resxPath = Path.Combine(artifactsRoot, "..", "Functions.Templates", "Resources", "Resources.resx");
            if (!File.Exists(resxPath))
            {
                // Try alternative path
                resxPath = Path.Combine(artifactsRoot, "Resources", "Resources.resx");
                if (!File.Exists(resxPath))
                {
                    Console.WriteLine($"Resources.resx not found at: {resxPath}");
                    return;
                }
            }

            Console.WriteLine($"Scanning templates from: {templatesRoot}");
            Console.WriteLine($"Reading resources from: {resxPath}\n");

            // Get all defined resource strings
            var definedStrings = GetResourceStringNamesFromResx(resxPath);
            Console.WriteLine($"Total resource strings defined in Resources.resx: {definedStrings.Count}");

            // Get referenced strings from metadata files
            var referencedFromMetadata = GetResourceStringNamesFromMetadataFiles(templatesRoot);
            Console.WriteLine($"Resource strings referenced in metadata.json files: {referencedFromMetadata.Count}");

            // Get referenced strings from bindings.json
            string bindingJsonLocation = Path.Combine(artifactsRoot, "bindings", "bindings.json");
            var referencedFromBindings = new HashSet<string>();
            if (File.Exists(bindingJsonLocation))
            {
                referencedFromBindings = new HashSet<string>(Helper.GetResourceStringNames(bindingJsonLocation));
                Console.WriteLine($"Resource strings referenced in bindings.json: {referencedFromBindings.Count}");
            }

            // Get referenced strings from templates.json
            string templateJsonLocation = Path.Combine(artifactsRoot, "templates", "templates.json");
            var referencedFromTemplates = new HashSet<string>();
            if (File.Exists(templateJsonLocation))
            {
                referencedFromTemplates = new HashSet<string>(Helper.GetResourceStringNames(templateJsonLocation));
                Console.WriteLine($"Resource strings referenced in templates.json: {referencedFromTemplates.Count}");
            }

            // Combine all referenced strings
            var allReferencedStrings = new HashSet<string>(referencedFromMetadata);
            allReferencedStrings.UnionWith(referencedFromBindings);
            allReferencedStrings.UnionWith(referencedFromTemplates);
            Console.WriteLine($"Total unique resource strings referenced: {allReferencedStrings.Count}\n");

            // Print unused strings
            PrintUnusedResourceStrings(definedStrings, allReferencedStrings, resxPath);
        }

        public static void PrintMissingResourceStrings(IDictionary<string, string> resourceMap, List<string> resourceStringNames, string ResourceFileName)
        {
            HashSet<string> missingResources = new HashSet<string>();
            foreach (var resourceStringName in resourceStringNames)
            {
                if (!resourceMap.Keys.Contains(resourceStringName))
                {
                    missingResources.Add(resourceStringName);
                }
            }

            if (missingResources.Count > 4)
            {
                Console.WriteLine($"Following items are missing in {ResourceFileName}\n Missing Count:{missingResources.Count}");
                foreach (var item in missingResources)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("\n\n");
                return;
            }

            Console.WriteLine($"Nothing missing in {ResourceFileName}. Everything looks good. \n\n");
        }
    }
}
