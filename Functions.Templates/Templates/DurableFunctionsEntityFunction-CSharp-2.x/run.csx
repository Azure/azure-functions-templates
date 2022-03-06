/*
* This function is not intended to be invoked directly. Instead it will be
* triggered by a client function.
* 
* Before running this sample, please:
* - create a Durable entity HTTP function
*/

#r "Microsoft.Azure.WebJobs.Extensions.DurableTask"

using Microsoft.Azure.WebJobs.Extensions.DurableTask;

public static void Run(IDurableEntityContext context)
{
    switch (context.OperationName.ToLowerInvariant())
    {
        case "add":
            context.SetState(context.GetState<int>() + context.GetInput<int>());
            break;
        case "reset":
            context.SetState(0);
            break;
        case "get":
            context.Return(context.GetState<int>());
            break;
    }
}