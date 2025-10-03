using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CheckResourceStrings
{
    partial class Program
    {
        /// <summary>
        /// Scans all metadata.json files in the templates directory and finds resource strings that are referenced
        /// </summary>
        public static HashSet<string> GetResourceStringNamesFromMetadataFiles(string templatesRoot)
        {
            var resources = new HashSet<string>();
            
            // Find all metadata.json files
            var metadataFiles = Directory.GetFiles(templatesRoot, "metadata.json", SearchOption.AllDirectories);
            
            foreach (var file in metadataFiles)
            {
                var resourceStrings = Helper.GetResourceStringNames(file);
                foreach (var resource in resourceStrings)
                {
                    resources.Add(resource);
                }
            }
            
            return resources;
        }

        /// <summary>
        /// Gets all resource string names defined in Resources.resx file
        /// </summary>
        public static HashSet<string> GetResourceStringNamesFromResx(string resxFilePath)
        {
            var resources = new HashSet<string>();
            
            if (!File.Exists(resxFilePath))
            {
                return resources;
            }

            try
            {
                var doc = XDocument.Load(resxFilePath);
                var dataElements = doc.Descendants("data");
                
                foreach (var element in dataElements)
                {
                    var nameAttr = element.Attribute("name");
                    if (nameAttr != null && !string.IsNullOrWhiteSpace(nameAttr.Value))
                    {
                        resources.Add(nameAttr.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading resx file: {ex.Message}");
            }
            
            return resources;
        }

        /// <summary>
        /// Prints unused resource strings found in Resources.resx
        /// </summary>
        public static void PrintUnusedResourceStrings(HashSet<string> definedStrings, HashSet<string> referencedStrings, string resourceFileName)
        {
            var unusedStrings = definedStrings.Except(referencedStrings).OrderBy(s => s).ToList();
            
            if (unusedStrings.Count == 0)
            {
                Console.WriteLine($"All resource strings in {resourceFileName} are being used. Everything looks good!\n\n");
                return;
            }
            
            Console.WriteLine($"Found {unusedStrings.Count} unused resource strings in {resourceFileName}:");
            Console.WriteLine("=".PadRight(80, '='));
            
            foreach (var str in unusedStrings)
            {
                Console.WriteLine(str);
            }
            
            Console.WriteLine("=".PadRight(80, '='));
            Console.WriteLine($"Total unused: {unusedStrings.Count}\n\n");
        }
    }
}
