## Settings for timer trigger

The settings provide a schedule expression. For example, the following schedule runs the function every minute:

 - `schedule`: Cron tab expression which defines schedule 
 - `name`: The variable name used in function code for the TimerTrigger. 
 - `type`: must be *timerTrigger*
 - `direction`: must be *in*

The timer trigger handles multi-instance scale-out automatically: only a single instance of a particular timer function will be running across all instances.

## Format of schedule expression

The schedule expression is a [CRON expression](http://en.wikipedia.org/wiki/Cron#CRON_expression) that includes 6 fields:  `{second} {minute} {hour} {day} {month} {day of the week}`. 

Note that many of the cron expressions you find online omit the {second} field, so if you copy from one of those you'll have to adjust for the extra field. 

Here are some other schedule expression examples:

To trigger once every 5 minutes:

```json
"schedule": "0 */5 * * * *"
```

To trigger once at the top of every hour:

```json
"schedule": "0 0 * * * *",
```

To trigger once every two hours:

```json
"schedule": "0 0 */2 * * *",
```

To trigger once every hour from 9 AM to 5 PM:

```json
"schedule": "0 0 9-17 * * *",
```

To trigger At 9:30 AM every day:

```json
"schedule": "0 30 9 * * *",
```

To trigger At 9:30 AM every weekday:

```json
"schedule": "0 30 9 * * 1-5",
```

## Timer trigger C# code example

This C# code example writes a single log each time the function is triggered.

```csharp
public static void Run(TimerInfo myTimer, TraceWriter log)
{
    log.Info($"C# Timer trigger function executed at: {DateTime.Now}");    
}
```

## Timer trigger JavaScript example

```JavaScript
module.exports = function(context, myTimer) {
    if(myTimer.isPastDue)
    {
        context.log('JavaScript is running late!');
    }
    context.log("Timer last triggered at " + myTimer.last);
    context.log("Timer triggered at " + myTimer.next);
    
    context.done();
}
```