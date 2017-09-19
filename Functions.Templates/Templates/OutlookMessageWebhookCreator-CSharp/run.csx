using System;
using System.Net;

public static HttpResponseMessage run(HttpRequestMessage req, out string clientState, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");
	clientState = Guid.NewGuid().ToString();
	return new HttpResponseMessage(HttpStatusCode.OK);
}