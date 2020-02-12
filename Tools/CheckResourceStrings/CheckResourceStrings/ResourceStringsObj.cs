using Newtonsoft.Json;
using System.Collections.Generic;

namespace CheckResourceStrings
{
    public class ResourceStringsObj
    {
        [JsonProperty(PropertyName = "lang")]
        public IDictionary<string, string> LanguageResourceMap { get; set; } = new Dictionary<string, string>();

        [JsonProperty(PropertyName = "en")]
        public IDictionary<string, string> EnglishResourceMap { get; set; } = new Dictionary<string, string>();
    }

}
