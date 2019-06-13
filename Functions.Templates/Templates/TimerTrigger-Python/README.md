# Python TimerTrigger

The Azure Functions Python Timer Trigger takes a [cron expression](https://en.wikipedia.org/wiki/Cron#CRON_expression) as input to schedule the Function's execution. For instance, the Azure Function can be scheduled to run every five minutes by passing the cron string `0 */5 * * * *`. The schedule of the function is set in the `function.json` file as the "schedule" value.

## Template Structure

``` text
<function-name>/    # directory holding all files for the TimerTrigger Function
└── __init__.py     # this module will be called when the Function is run
    function.json   # defines the schedule for calling __init__.py
    README.md       # contains documentation on how to run and edit the TimerTrigger Function
```
