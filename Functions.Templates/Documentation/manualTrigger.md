#### Settings for manual trigger

- `type` : Must be set to *manualTrigger*.
- `name` : The variable name used in function code for the event hub message. 

#### Data types

Manual trigger supports
 - Object (JSON) (C# supports `T`)
 - String

#### Manual trigger C# example
 
 ```csharp
using System;

public static void Run(string trigger, TraceWriter log)
{
    log.Info(trigger);
}
```

#### Manual trigger JavaScript example
 
 ```javascript
module.exports = function (context, trigger) {
    context.log(trigger);	
    context.done();
};
```