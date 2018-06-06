package $packageName$;

import com.microsoft.azure.functions.*;
import com.microsoft.azure.functions.annotation.*;

public class BlobTriggers {
    @FunctionName("$functionName$")
    @StorageAccount("$connection$")
    public static byte[] run(
        @BlobTrigger(name="blob", dataType="binary", path="$path$") byte[] content,
        @BindingName("name") String name,
        final ExecutionContext context
    ) {
        context.getLogger().info("Java Blob trigger function processed a blob.\n  Name: " + name + 
            "\n  Size: " + content.length + " Bytes");
        return content;
    }
}