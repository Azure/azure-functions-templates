#### Settings for DocumentDB input binding

- `name` : Variable name used in function code for the document.
- `type` : must be set to "documentdb".
- `databaseName` : The database containing the document.
- `collectionName` : The collection containing the document.
- `id` : The Id of the document to retrieve. This property supports bindings similar to "{queueTrigger}", which will use the string value of the queue message as the document Id.
- `connection` : This string must be an Application Setting set to the endpoint for your DocumentDB account. 
- `direction`  : must be set to *"in"*.

#### Azure DocumentDB input code example for a C# queue trigger
 
The DocumentDB input binding will retrieve the document with the id that matches the queue message string and pass it to the 'document' parameter. If that document is not found, the 'document' parameter will be null. The document is then updated with the new text value when the function exits.
 
 ```csharp
public static void Run(string myQueueItem, dynamic document)
{   
    document.text = "This has changed.";
}
```

#### Azure DocumentDB input code example for a JavaScript queue trigger
 
The DocumentDB input binding will retrieve the document with the id that matches the queue message string and pass it to the `documentIn` binding property. In JavaScript functions, updated documents are not sent back to the collection. However, you can pass the input binding directly to a DocumentDB output binding named `documentOut` to support updates. This code example updates the text property of the input document and sets it as the output document.
 
 ```javascript
module.exports = function (context, input) {   
    context.bindings.documentOut = context.bindings.documentIn;
    context.bindings.documentOut.text = "This was updated!";
    context.done();
};
```