using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.MySql;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class MySqlInputBindingHttpTriggerCSharp
{
    private readonly ILogger _logger;

    public MySqlInputBindingHttpTriggerCSharp(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<MySqlInputBindingHttpTriggerCSharp>();
    }

    [Function("MySqlInputBindingHttpTriggerCSharp")]
    public IEnumerable<Object> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
        [MySqlInput("SELECT * FROM object",
        "MySqlConnectionString")] IEnumerable<Object> result)
    {
        _logger.LogInformation("C# HTTP trigger with MySQL Input Binding function processed a request.");

        return result;
    }
}