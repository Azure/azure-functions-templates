/*
 * Before running this sample, please create a Durable Activity function (default name is "hello")
 */

#r "Microsoft.Azure.WebJobs.Extensions.DurableTask"

public static string Run(string name)
{
    return $"Hello {name}!";
}