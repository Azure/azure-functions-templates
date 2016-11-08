#### function.json for storage tables

The *function.json* specifies the following properties.

- `name` : The variable name used in function code for the table binding. 
- `tableName` : The name of the table.
- `partitionKey` and `rowKey` : Used together to read to write a single entity in a JavaScript function.
- `connection` : The name of an app setting that contains a storage connection string. If you leave `connection` empty, the binding will work with the default storage connection string for the function app, which is specified by the AzureWebJobsStorage app setting.
- `type` : Must be set to *table*.
- `direction` : Set to *out*. 

#### Storage tables input and output supported types

The `table` binding can serialize or deserialize objects in JavaScript or C# functions. The objects will have RowKey and PartitionKey properties. 

In C# functions, you can also bind to the following types:

* `T` where `T` implements `ITableEntity`
* `ICollector<T>` 
* `IAsyncCollector<T>`

#### Storage tables binding scenarios

The table binding supports the following scenarios:

* Write one or more rows in a C# function.

	The Functions runtime provides an `ICollector<T>` or `IAsyncCollector<T>` bound to the table, where `T` specifies the schema of the entities you want to add. Typically, type `T` derives from `TableEntity` or implements `ITableEntity`, but it doesn't have to. The `partitionKey`, `rowKey`, `filter`, and `take` properties are not used in this scenario.


#### Storage tables example: C# code that creates table entities

```csharp
public static void Run(string input, ICollector<Person> tableBinding, TraceWriter log)
{
    for (int i = 1; i < 10; i++)
        {
            log.Info($"Adding Person entity {i}");
            tableBinding.Add(
                new Person() { 
                    PartitionKey = "Test", 
                    RowKey = i.ToString(), 
                    Name = "Name" + i.ToString() }
                );
        }

}

public class Person
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string Name { get; set; }
}

```

#### Storage tables example: JavaScript example that creates a table entity

```javascript
module.exports = function (context, myQueueItem) {
    context.log('JavaScript queue trigger function processed work item', myQueueItem);
    context.bindings.tableOut = {
        "partitionKey": "123Contoso",
        "rowKey":"Fabrikam43234",
        "Name": "Name" + myQueueItem 
    }
    context.done();
};
```
