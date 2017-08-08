### MS Graph OneDrive Binding

#### Summary
This input binding can be used to retrieve the contents of a file stored in a user's OneDrive. The authentication information provided must correspond to a user with access to the desired file.

#### Settings
The settings specify the following properties.

- `name` : The variable name used in function code for the OneDrive file. 
- `direction` : Must be set to *in*. 
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
      "schedule": "0 0 3 * * *"
    },
    {
      "type": "onedrive",
      "name": "fileInput",
      "Path": "Documents/test.txt",
      "PrincipalId": "appsetting_principal_id",
      "direction": "in"
    }
  ],
  "disabled": false
}
```

#### Language Support

##### C# 

###### Example code
```csharp
public static void Run(TimerInfo timer, TraceWriter log, Stream fileInput)
{
    StreamReader reader = new StreamReader(fileInput);
    log.Info($"File contents: {reader.ReadToEnd()}");
}
```

###### Supported types
Input files can be bound to any of the following types:

* byte[]
* Stream
* DriveItem

##### Python
###### Example function.json
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
      "type": "http",
      "direction": "out",
      "name": "res"
    },
    {
      "type": "onedrive",
      "name": "fileInput",
      "Path": "Documents/test.txt",
      "PrincipalId": "{principalID}",
      "direction": "in"
    }
  ],
  "disabled": false
}
```
###### Example code
```python
import os
import json

# Write entire contents of file to HTTP response
response = open(os.environ['res'], 'w')
data = open(os.environ['fileInput']).read()
response.write(data);
response.close()
```

```python
import os
import json

# Read json file and write a key, value pair to HTTP response
response = open(os.environ['res'], 'w')
data = json.loads(open(os.environ['fileInput']).read())
response.write("testKey: " + data['testKey']);
response.close()
```

###### Supported types
Input files can be bound to any of the following types:

* string