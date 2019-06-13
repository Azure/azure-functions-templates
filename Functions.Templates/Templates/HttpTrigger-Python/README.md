# Python HttpTrigger

The Azure Functions Python HTTP Trigger creates an endpoint: `<function-host>/api/<function-name>`. When an HTTP request is made against that endpoint, the `main()` function in `__init__.py` is called.

> Throughout this documentation variables in angle brackets `<>` are placeholders and will be different in your application.

## Default Structure

``` text
<function-name>/      # directory holding all files for the HttpTrigger Function
└── __init__.py     # this module will be called when the Function is run
    function.json   # defines that __init__.py will be called and which bindings are used
    sample.dat      # contains a sample input for the Function
```

## Testing the Template Function

The default function template when a new HttpTrigger is created takes a value for `name` and returns `Hello <name>!`. To test this functionality, from the root of the Azure Function App, execute

``` bash
func host start
```

Once running, you should see

``` text
Http Functions:

        <function-name>: [GET,POST] http://localhost:7071/api/<function-name>
```

Open the URL in a browser. You should get the response

``` text
Please pass a name on the query string or in the request body
```

To the URL, append `?name=Sophia` so it reads

``` text
http://localhost:7071/api/<function-name>?name=Sophia
```

You should now get the response

``` text
Hello Sophia!
```

## Useful Resources

- Software: [Postman](https://www.getpostman.com/) is a cross-platform application for testing HTTP requests.
- Guide: [Create an HTTP triggered function in Azure](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-function-python)  
- Examples: [Serverless Library - Python](https://serverlesslibrary.net/?technology=Functions%202.x&language=Python)
