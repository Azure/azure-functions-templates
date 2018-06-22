package $packageName$;

import java.util.*;
import com.microsoft.azure.functions.annotation.*;
import com.microsoft.azure.functions.*;

/**
 * Http trigger templates for Microsoft Azure Functions.
 */
public class HttpTriggers {
    /**
     * It will generate a RESTful API endpoint "/api/echo". There are two ways to invoke it using "curl" command in bash:
     *   1. curl -d "Http Body" {your host}/api/echo
     *   2. curl {your host}/api/echo?name=Http%20Query
     */
    @FunctionName("$functionName$")
    public HttpResponseMessage<String> run(
        @HttpTrigger(name = "req", methods = { "get", "post" }, authLevel = AuthorizationLevel.$authLevel$) HttpRequestMessage<Optional<String>> request,
        ExecutionContext context
    ) {
        context.getLogger().info("Java HTTP trigger processed a request.");

        String query = request.getQueryParameters().get("name");
        String name = request.getBody().orElse(query);

        if (name == null) {
            return request.createResponse(400, "Please pass a name on the query string or in the request body");
        } else {
            return request.createResponse(200, "Hello, " + name);
        }
    }
}