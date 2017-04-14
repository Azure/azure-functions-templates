using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string name, TraceWriter log)
{    
    log.Info("C# HTTP trigger function processed a request.");
    
    // Fetching the name from the path parameter in the request URL
    return req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
}