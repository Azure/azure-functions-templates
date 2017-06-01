#### Settings for storage tables

- `name` : The variable name used in function code for the table binding. 
- `tableName` : The name of the table.
- `partitionKey` and `rowKey` : Used together to read a single entity in a C# or JavaScript function
- `take` : The maximum number of rows to read for table input in a JavaScript function.
- `filter` : OData filter expression for table input in a JavaScript function.
- `connection` : The name of an app setting that contains a storage connection string. 
- `type` : Must be set to *table*.
- `direction` : Set to *in*

#### Storage tables input supported types

The `table` binding can serialize or deserialize objects in JavaScript or C# functions. The objects will have RowKey and PartitionKey properties. 

In C# functions, you can also bind to the following types:

* `T` where `T` implements `ITableEntity`
* `IQueryable<T>` 

#### Storage tables binding scenarios

The table binding supports the following scenarios:

* Read a single row in a C# or JavaScript function.

	Set `partitionKey` and `rowKey`. The `filter` and `take` properties are not used in this scenario.

* Read multiple rows in a C# function.

	The Functions runtime provides an `IQueryable<T>` object bound to the table. Type `T` must derive from `TableEntity` or implement `ITableEntity`. The `partitionKey`, `rowKey`, `filter`, and `take` properties are not used in this scenario; you can use the `IQueryable` object to do any filtering required. 

* Read multiple rows in a JavaScript function.

	Set the `filter` and `take` properties. Don't set `partitionKey` or `rowKey`.


#### Storage tables example: Read a single table entity in C# or JavaScript

The queue message has the row key value and the table entity is read into a type that is user defined. The type includes `PartitionKey` and `RowKey` properties and does not derive from `TableEntity`. 

```csharp
public static void Run(string myQueueItem, Person personEntity, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");
    log.Info($"Name in Person entity: {personEntity.Name}");
}

public class Person
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string Name { get; set; }
}
```

The following JavaScript code example also works to read a single table entity.

```javascript
module.exports = function (context, myQueueItem) {
    context.log('JavaScript queue trigger function processed work item', myQueueItem);
    context.log('Person entity name: ' + context.bindings.personEntity.Name);
    context.done();
};
```

#### Storage tables example: C# example that reads multiple table entities

The C# code adds a reference to the Azure Storage SDK so that the entity type can derive from `TableEntity`.

```csharp
#r "Microsoft.WindowsAzure.Storage"
using Microsoft.WindowsAzure.Storage.Table;

public static void Run(string myQueueItem, IQueryable<Person> tableBinding, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");
    foreach (Person person in tableBinding.Where(p => p.PartitionKey == myQueueItem).ToList())
    {
        log.Info($"Name: {person.Name}");
    }
}

public class Person : TableEntity
{
    public string Name { get; set; }
}
``` 
