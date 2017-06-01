#### Settings for DocumentDB input binding

- `name` : Variable name used in function code for the document.
- `type` : must be set to "documentdb".
- `databaseName` : The database containing the document.
- `collectionName` : The collection containing the document.
- `id` : The Id of the document to retrieve. This property supports bindings similar to `{queueTrigger}`, which will use the string value of the queue message as the document Id.
- `sqlQuery` : A DocumentDB SQL query used for retrieving multiple documents. The query supports runtime bindings. For example: `SELECT * FROM c where c.departmentId = {departmentId}`
- `connection` : This string must be an Application Setting set to the endpoint for your DocumentDB account. 
- `direction`  : must be set to *"in"*.

The properties `id` and `sqlQuery` cannot be set at the same time. If neither `id` nor `sqlQuery` is set, the entire collection is retrieved.

#### Azure DocumentDB single document input code example for a C# queue trigger

In this example, the DocumentDB input binding will retrieve the document with the id that matches the queue message string and pass it to the 'document' parameter. If that document is not found, the 'document' parameter will be null. The document is then replaced with the modified document when the function exits.

Input binding settings:
```javascript
{
    "name": "document",
    "type": "documentdb",
    "direction": "in",
    "databaseName": "MyDb",
    "collectionName": "MyCollection",
    "id": "{queueTrigger}"
    "connection": "DocumentDBConnection"
}
```

C# code:
```csharp
public static void Run(string myQueueItem, dynamic document)
{   
    document.text = "This has changed.";
}
```

#### Azure DocumentDB input code example for a JavaScript queue trigger

In this example, the DocumentDB input binding will retrieve the document with the id that matches the queue message string and pass it to the `documentIn` binding property. In JavaScript functions, updated documents are not sent back to the collection. However, you can pass the input binding directly to a DocumentDB output binding named `documentOut` to support updates. This code example updates the text property of the input document and sets it as the output document.
 

Input binding settings:
```javascript
{
    "name": "documentIn",
    "type": "documentdb",
    "direction": "in",
    "databaseName": "MyDb",
    "collectionName": "MyCollection",
    "id": "{queueTrigger}"
    "connection": "DocumentDBConnection"
}
```

JavaScript code:
```javascript
module.exports = function (context, input) {   
    context.bindings.documentOut = context.bindings.documentIn;
    context.bindings.documentOut.text = "This was updated!";
    context.done();
};
```

#### Azure DocumentDB multiple document input code example for a C# queue trigger
 
In this example, the DocumentDB input binding will retrieve all documents returned by the specified `sqlQuery`. The `departmentId` value is automatically inserted to the query from the input trigger. For example, a queue message of `{ "departmentId" : "Finance" }` would return all records for the Finance department.

Input binding settings:
```javascript
{
    "name": "documents",
    "type": "documentdb",
    "direction": "in",
    "databaseName": "MyDb",
    "collectionName": "MyCollection",
    "sqlQuery": "SELECT * from c where c.departmentId = {departmentId}"
    "connection": "DocumentDBConnection"
}
```

C# code
```csharp
public static void Run(QueuePayload myQueueItem, IEnumerable<dynamic> documents)
{   
    foreach (var doc in documents)
    {
        // operate on each document
    }    
}

public class QueuePayload
{
    public string departmentId { get; set; }
}
```

#### Azure DocumentDB multiple document input code example for a JavaScript queue trigger
 
In this example, the DocumentDB input binding will retrieve all documents returned by the specified `sqlQuery`. The `departmentId` value is automatically inserted to the query from the input trigger. For example, a queue message of `{ "departmentId" : "Finance" }` would return all records for the Finance department.

Input binding settings:
```javascript
{
    "name": "documents",
    "type": "documentdb",
    "direction": "in",
    "databaseName": "MyDb",
    "collectionName": "MyCollection",
    "sqlQuery": "SELECT * from c where c.departmentId = {departmentId}"
    "connection": "DocumentDBConnection"
}
```

JavaScript code:
```javascript
module.exports = function (context, input) {    
    var documents = context.bindings.documents;
    for (var i = 0; i < documents.length; i++) {
        var document = documents[i];
        // operate on each document
    }	    
    context.done();
};
```