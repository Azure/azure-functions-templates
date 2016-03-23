#r "Newtonsoft.Json"

using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    // parse query parameter
    var queryParam = req.GetQueryNameValuePairs()
                 .ToDictionary(p => p.Key, p => p.Value, StringComparer.OrdinalIgnoreCase);

    log.Verbose($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // Get request body
    string jsonContent = req.Content.ReadAsStringAsync().Result;
    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    HttpResponseMessage res = null;
    string name;

    // Get query string parameter
    bool getQueryParam = queryParam.TryGetValue("name", out name);

    // if no query string found and no request body found display error message
    if ((data == null || data.name == null) && !getQueryParam)
    {
        res = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Please pass a name on the query string or in the request body")
        };
    }
    else
    {
        name = getQueryParam ? name : data.name;
        res = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("Hello " + name)
        };
    }

    return Task.FromResult(res);
}
