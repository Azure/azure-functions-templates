#### Settings for MS Graph OneDrive Binding
The settings specify the following properties.

- `name` : The variable name used in function code for the OneDrive file. 
- `direction` : Must be set to *out*. 
- `Type` : Must be set to *OneDrive*.
- `Path` : Path from root OneDrive directory to file (e.g. Documents/test.txt).
- `PrincipalId` : Should be set to either an app setting containing the Principal id/OID to be used to communicate with MS Graph or an expression to evaluate to a Principal id/OID
- `idToken` : Should be set to an expression that evaluates to an id token. Either Principal id or id token must be set, but not both.
#### Example function.json
```json
{
  "bindings": [
    {
      "type": "timerTrigger",
      "direction": "in",
      "name": "timer",
      "schedule": "0 45 9 * * *"
    },
    {
      "type": "onedrive",
      "name": "output",
      "Path": "Documents/test.txt",
      "PrincipalId": "%Identity.<alias>%",
      "direction": "out"
    }
  ],
  "disabled": false
}
```

#### C# Example code
```csharp
using System.Text;

// Update contents of file
public static void Run(TimerInfo timer, TraceWriter log, out byte[] file)
{
	file = Encoding.UTF8.GetBytes("Update contents of specified file");
}
```

#### Supported types

[Output] File data can be serialized to any of the following types:

* byte[]
* Stream
