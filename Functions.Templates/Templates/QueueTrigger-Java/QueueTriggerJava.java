package $packageName$;

import com.microsoft.azure.functions.*;
import com.microsoft.azure.functions.annotation.*;

public class QueueTriggers {
    @FunctionName("$functionName$")
    public static void run(
        @QueueTrigger(name="msg", queueName="$queueName$", connection="$connection$") String message,
        final ExecutionContext context
    ) {
        context.getLogger().info("Java Queue trigger function processed message: " + message);
    }
}