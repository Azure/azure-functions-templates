#### Settings for Redis Input Binding

See [Azure Functions Redis Extension](https://github.com/Azure/azure-functions-redis-extension) for general information about all Redis Bindings.

The settings for a `RedisInput` binding specifies the following properties:

- `type` : Must be set to *redis*.
- `name` : The name of the parameter that the returned value binds to.
- `direction` : Must be set to *in*.
- `connection` : The name of an app setting that contains the Redis connection information. See the [available connection types](https://github.com/Azure/azure-functions-redis-extension?tab=readme-ov-file#connection-types) for more information.
- `command` : The redis-cli command to be executed on the cache with all arguments separated by spaces. (e.g. `"GET key"`, `"HGET key field"`)

#### Redis Input Binding C# code example

This C# code gets any key that was recently set when keyspace notifications are enabled. Please refer to our [samples](https://github.com/Azure/azure-functions-redis-extension/tree/main/samples) for more examples.

```csharp
[FunctionName(nameof(SetGetter))]
public static void SetGetter(
    [RedisPubSubTrigger("Redis", "__keyevent@0__:set")] ChannelMessage message,
    [Redis("Redis", "GET {Message}")] string value,
    ILogger logger)
{
    logger.LogInformation($"Key '{message.Message}' was set to value '{value}'");
}
```
