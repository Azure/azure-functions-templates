/*
* This function is not intended to be invoked directly. Instead it will be
* triggered by a client function.
* 
* Before running this sample, please:
* - create a Durable entity HTTP function
*/

#r "Microsoft.Azure.WebJobs.Extensions.DurableTask"
#r "Newtonsoft.Json"

using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

public class Counter
{

    [JsonProperty("value")]
    public int CurrentValue { get; set; }

    public void Add(int amount) => this.CurrentValue += amount;

    public void Reset() => this.CurrentValue = 0;

    public int Get() => this.CurrentValue;
}

public static void Run(IDurableEntityContext context)
    => context.DispatchAsync<Counter>();