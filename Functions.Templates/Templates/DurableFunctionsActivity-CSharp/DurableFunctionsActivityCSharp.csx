#r "Microsoft.Azure.WebJobs.Extensions.DurableTask"

public static string Run(string name)
{
    return $"Hello {name}!";
}