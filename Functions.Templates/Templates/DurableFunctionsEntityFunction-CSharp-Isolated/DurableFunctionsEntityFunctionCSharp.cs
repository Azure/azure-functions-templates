using Microsoft.Azure.Functions.Worker;

namespace Company.Function;

public static class DurableFunctionsEntityFunctionCSharp
{
    [Function("Counter")]
    public static Task DispatchAsync([EntityTrigger] TaskEntityDispatcher dispatcher)
    {
        return dispatcher.DispatchAsync(operation =>
        {
            if (operation.State.GetState(typeof(int)) is null)
            {
                operation.State.SetState(0);
            }

            switch (operation.Name.ToLowerInvariant())
            {
                case "add":
                    int state = operation.State.GetState<int>();
                    state += operation.GetInput<int>();
                    operation.State.SetState(state);
                    return new(state);
                case "reset":
                    operation.State.SetState(0);
                    break;
                case "get":
                    return new(operation.State.GetState<int>());
                case "delete":
                    operation.State.SetState(null);
                    break;
            }

            return default;
        });
    }
}