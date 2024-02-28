using Microsoft.Azure.Functions.Worker.Extensions.Redis;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class RedisPubSubTriggerCSharp
    {
        private readonly ILogger<RedisPubSubTriggerCSharp> logger;

        public RedisPubSubTriggerCSharp(ILogger<RedisPubSubTriggerCSharp> logger)
        {
            this.logger = logger;
        }

        [Function(nameof(RedisPubSubTriggerCSharp))]
        public static void Run(
            [RedisPubSubTrigger("RedisConnectionString", "redisChannel")] ChannelMessage message,
            ILogger log)
        {
            log.LogInformation($"Message '{message.Message}' was pushed to channel '{message.Channel}'");
        }
    }

    public class ChannelMessage
    {
        public string SubscriptionChannel { get; set; }
        public string Channel { get; set; }
        public string Message { get; set; }
    }
}
