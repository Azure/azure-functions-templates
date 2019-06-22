# Python HttpTrigger

The HTTP trigger lets you invoke a function with an HTTP request. You can use an HTTP trigger to build serverless APIs and respond to webhooks. It creates an endpoint like  `https://<app-name>.azurewebsites.net/api/api/<function-name>`. When an HTTP request is made against that endpoint, the `main()` function in `__init__.py` is called.


> Throughout this documentation variables in angle brackets `<>` are placeholders and will be different in your application.

## Default Structure

``` text
<function-name>/    # directory holding all files for the HttpTrigger Function
└── __init__.py     # this module will be called when the Function is run
    function.json   # defines that __init__.py will be called and which bindings are used
    README.md       # documentation for using HTTP Trigger
    sample.dat      # contains a sample input for the Function
```

## Run the Function Locally

The default function template when a new HttpTrigger is created takes a value for `name` and returns `Hello <name>!`. If you're running in VS Code, you can press F5 to test your function locally. Otherwise, from the root of the Azure Function App, execute

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

## Resources

- [Azure Functions Python developer reference](https://aka.ms/python-functions-guide)  
- [HTTP triggers and bindings](https://docs.microsoft.com/azure/azure-functions/functions-bindings-http-webhook)
- [Serverless Library](https://aka.ms/python-functions-samples)
