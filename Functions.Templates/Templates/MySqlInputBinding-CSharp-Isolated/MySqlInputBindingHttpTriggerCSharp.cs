using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.MySql;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class MySqlInputBindingHttpTriggerCSharp(ILogger<MySqlInputBindingHttpTriggerCSharp> logger)
{
    [Function("MySqlInputBindingHttpTriggerCSharp")]
    public IEnumerable<Object> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        [MySqlInput("SELECT * FROM object",
        "MySqlConnectionString")] IEnumerable<Object> result)
    {
        logger.LogInformation("C# HTTP trigger with MySQL Input Binding function processed a request.");

        return result;
    }
}