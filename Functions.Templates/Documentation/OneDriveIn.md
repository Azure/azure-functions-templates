#### Settings for MS Graph OneDrive Binding
The settings specify the following properties.

- `name` : The variable name used in function code for the OneDrive file. 
- `direction` : Must be set to *in*. 
- `Type` : Must be set to *OneDrive*.
- `Path` : Path from root OneDrive directory to file (e.g. Documents/test.txt).
- `PrincipalId` : Should be set to either an app setting containing the Principal id/OID to be used to communicate with MS Graph or an expression to evaluate to a Principal id/OID
- `idToken` : Should be set to an expression that evaluates to an id token. Either Principal id or id token must be set, but not both.

#### Example bindings.json
```json
{
  "bindings": [
    {
      "type": "timerTrigger",
      "direction": "in",
      "name": "timer",
      "schedule": "0 0 3 * * *"
    },
    {
      "type": "onedrive",
      "name": "file",
      "Path": "Documents/test.txt",
      "PrincipalId": "%stored_principal_id%",
      "direction": "in"
    }
  ],
  "disabled": false
}
```

#### C# Example code
```csharp
public static void Run(TimerInfo timer, TraceWriter log, Stream file)
{
    StreamReader reader = new StreamReader(file);
    log.Info($"File contents: {reader.ReadToEnd()}");
}
```

#### Supported types

[Input] File data can be deserialized to any of the following types:

* byte[]
* Stream
* DriveItem