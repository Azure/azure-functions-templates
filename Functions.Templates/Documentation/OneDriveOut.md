### MS Graph OneDrive Binding

#### Summary
This output binding can be used to update the contents of a file stored in a user's OneDrive. The authentication information provided must correspond to a user with access to the desired file.

#### Settings for MS Graph OneDrive Binding
The settings specify the following properties.

- `name` : The variable name used in function code for the OneDrive file. 
- `direction` : Must be set to *out*. 
- `Type` : Must be set to *OneDrive*.
- `Path` : Path from root OneDrive directory to file (e.g. Documents/test.txt).
- `PrincipalId` : Should be set to either an app setting containing the Principal ID/OID to be used to communicate with MS Graph or an expression to evaluate to a Principal ID/OID
- `idToken` : Should be set to an expression that evaluates to an ID token. Either Principal ID or ID token must be set, but not both.

#### Example function.json
function.json is language independent, but not all triggers are supported by all languages.
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

#### Language Support
##### C# 

###### Example code
```csharp
using System.Text;

// Update contents of file
public static void Run(TimerInfo timer, TraceWriter log, out byte[] file)
{
	file = Encoding.UTF8.GetBytes("Update contents of specified file");
}
```

###### Supported types

The data used to update a file can come from any of the following types:

* byte[]
* Stream

##### Python
###### function.json
```json
{
  "bindings": [
    {
      "authLevel": "anonymous",
      "type": "httpTrigger",
      "direction": "in",
      "name": "req",
      "methods": [
        "post"
      ]
    },
    {
      "type": "onedrive",
      "name": "fileOutput",
      "Path": "Documents/test.txt",
      "PrincipalId": "Identity.alias",
      "direction": "out"
    }
  ],
  "disabled": false
}
```

###### Example code

```python
import os
import json

# Write value of json property of HTTP Request to file
postreqdata = json.loads(open(os.environ['req']).read()) # read HTTP request

data = open(os.environ['fileOutput'], 'w')
data.write(postreqdata['testProperty'])
data.close()
```

###### Supported types
Output file data can come from any of the following types:

* string