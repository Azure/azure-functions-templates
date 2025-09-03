# Azure Functions HTTP trigger (updated)

Use the HTTP trigger to invoke a function with an HTTP request. Typical uses include building serverless APIs and responding to webhooks.

- Default responses: Functions 2.x and later return HTTP 204 No Content when your function doesn’t write a response; Functions 1.x returned HTTP 200 OK.
- To shape the HTTP response, use the HTTP output binding or the language’s response APIs.

Authoritative docs: [HTTP trigger reference](https://learn.microsoft.com/azure/azure-functions/functions-bindings-http-webhook-trigger)

## Binding settings

HTTP trigger (request) properties:

- `name`: Variable name used in function code for the request or request body.
- `type`: `httpTrigger`.
- `direction`: `in`.
- `authLevel`: Authorization level; one of `anonymous`, `function`, or `admin`.
- `methods`: Optional array of allowed HTTP methods (for example, `["get", "post"]`). If omitted, all methods are allowed.
- `route`: Optional route template that customizes the URL. Defaults to `<functionname>`.

HTTP output (response) properties:

- `name`: Variable name used for the response.
- `type`: `http`.
- `direction`: `out`.

By default, function routes are prefixed with `api`. You can change or remove this prefix with `extensions.http.routePrefix` in `host.json`.

## URL to trigger the function

Default URL shape:

```text
https://<APP_NAME>.azurewebsites.net/api/<FUNCTION_NAME>
```

You can customize the route (including parameters and constraints) via the `route` property, such as `products/{category:alpha}/{id:int?}`.

## Authorization and access keys

Unless `authLevel` is set to `anonymous`, requests must include a valid access key. Provide the key either:

- As a query parameter: `?code=<FUNCTION_OR_HOST_KEY>`
- As a header: `x-functions-key: <FUNCTION_OR_HOST_KEY>`

Notes:

- `authLevel` values:
  - `anonymous`: No key required.
  - `function`: A function-specific key (or any host key) required.
  - `admin`: The host master key required. Avoid using `admin` for clients.
- When not specified, authorization typically defaults to `function` (Node.js model v4 defaults to `anonymous`).
- When running locally (Core Tools), authorization is disabled; after publishing to Azure, the setting is enforced. Keys are still required when running locally in a container.
- Manage keys using the Azure portal, CLI, or ARM/REST—don’t store keys in code or in `host.json`.

Learn more: [Work with access keys in Azure Functions](https://learn.microsoft.com/azure/azure-functions/function-keys-how-to)

## Security best practices

Guidance - [Securing Azure Functions](https://learn.microsoft.com/azure/azure-functions/security-concepts)

## Customize the HTTP endpoint (route)

Define a `route` template with tokens and constraints. The following examples show modern patterns.

### C# (.NET isolated worker)

```csharp
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

public class Products
{
        [Function("GetProduct")]
        public static HttpResponseData Run(
                [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{category:alpha}/{id:int?}")] HttpRequestData req,
                string category,
                int? id)
        {
                var res = req.CreateResponse(HttpStatusCode.OK);
                res.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                res.WriteString($"Category: {category}, ID: {id}");
                return res;
        }
}
```

### JavaScript/TypeScript (Node.js model v4)

```javascript
const { app } = require('@azure/functions');

app.http('getProduct', {
    methods: ['GET'],
    authLevel: 'function',
    route: 'products/{category:alpha}/{id:int?}',
    handler: async (request, context) => {
        const { category, id } = request.params;
        return { body: `Category: ${category}, ID: ${id}` };
    }
});
```

For v3 model apps, define `route` in `function.json`:

```json
{
    "bindings": [
        {
            "type": "httpTrigger",
            "direction": "in",
            "name": "req",
            "methods": ["get"],
            "route": "products/{category:alpha}/{id:int?}"
        },
        { "type": "http", "direction": "out", "name": "res" }
    ]
}
```

## References

- HTTP trigger reference: [HTTP trigger](https://learn.microsoft.com/azure/azure-functions/functions-bindings-http-webhook-trigger)
- Function keys: [Work with access keys](https://learn.microsoft.com/azure/azure-functions/function-keys-how-to)
- Security overview: [Securing Azure Functions](https://learn.microsoft.com/azure/azure-functions/security-concepts)
