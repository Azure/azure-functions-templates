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
