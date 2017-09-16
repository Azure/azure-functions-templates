using System;
using System.Net;

public static HttpResponseMessage run(HttpRequestMessage req, out string clientState)
{
	clientState = Guid.NewGuid().ToString();
	return new HttpResponseMessage(HttpStatusCode.OK);
}