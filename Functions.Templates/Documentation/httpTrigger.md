## Settings for HTTP bindings

The settings provide properties that pertain to both the request and response.

Properties for the HTTP request:

- `name` : Variable name used in function code for the request object (or the request body in the case of JavaScript functions).
- `type` : Must be set to *httpTrigger*.
- `direction` : Must be set to *in*. 
- `authLevel` : Set to "function" to require the API key, "anonymous" to drop the API key requirement, or "admin" to require the master API key.

Properties for the HTTP response:

- `name` : Variable name used in function code for the response object.
- `type` : Must be set to *http*.
- `direction` : Must be set to *out*. 

## URL to trigger the function

To trigger a function, you send an HTTP request to a URL that is a combination of the function app URL and the function name:

```
 https://{function app name}.azurewebsites.net/api/{function name} 
```

## API keys

By default, an API key must be included with an HTTP request to trigger an HTTP function. The key can be included in a query string variable named `code`, or it can be included in an `x-functions-key` HTTP header. You can indicate that an API key is not required by setting the `authLevel` property to "anonymous" in the *function.json* file.

You can find API key values in the *D:\home\data\Functions\secrets* folder in the file system of the function app.  The master key and function key are set in the *host.json* file, as shown in this example. 

```json
{
  "masterKey": "K6P2VxK6P2VxK6P2VxmuefWzd4ljqeOOZWpgDdHW269P2hb7OSJbDg==",
  "functionKey": "OBmXvc2K6P2VxK6P2VxK6P2VxVvCdB89gChyHbzwTS/YYGWWndAbmA=="
}
```

The function key from *host.json* can be used to trigger any function but won't trigger a disabled function. The master key can be used to trigger any function and will trigger a function even if it's disabled. You can configure a function to require the master key by setting the `authLevel` property to "admin". 

If the *secrets* folder contains a JSON file with the same name as a function, the `key` property in that file can also be used to trigger the function, and this key will only work with the function it refers to. For example, the API key for a function named `HttpTrigger` is specified in *HttpTrigger.json* in the *secrets* folder. Here is an example:

```json
{
  "key":"0t04nmo37hmoir2rwk16skyb9xsug32pdo75oce9r4kg9zfrn93wn4cx0sxo4af0kdcz69a4i"
}
```

## Example C# code for an HTTP trigger function 

```csharp
using System.Net;
using System.Threading.Tasks;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    // parse query parameter
    string name = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
        .Value;

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    // Set name to query string or body data
    name = name ?? data?.name;

    return name == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
        : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
}
```

## Example JavaScript code for an HTTP trigger function 

We support an [express-like api](https://expressjs.com/en/4x/api.html#res) for JavaScript http triggers.
See supported methods for [context.req](https://github.com/Azure/azure-functions-host/blob/v1.x/src/WebJobs.Script/azurefunctions/http/request.js) and [context.res](https://github.com/Azure/azure-functions-host/blob/v1.x/src/WebJobs.Script/azurefunctions/http/response.js).

```javascript
module.exports = function(context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    if (req.query.name || (req.body && req.body.name)) {
        // using the express api style
        context.res
            // set statusCode to 200
            .status(200)
            // set a header on the response
            .set("QuerySet", req.query.name != undefined)
            // send will automatically call context.done
            .send("Hello " + (req.query.name || req.body.name));
    } else {
        // alternate style
        context.res = {
            status: 400,
            body: "Please pass a name on the query string or in the request body"
        };
        context.done();
    }
};
```
