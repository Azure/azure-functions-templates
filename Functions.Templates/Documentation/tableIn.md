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

#### Storage tables binding scenarios

The table binding supports the following scenarios:

* Read a single row in a C# or JavaScript function.

	Set `partitionKey` and `rowKey`. The `filter` and `take` properties are not used in this scenario.

* Read multiple rows in C# function and JavaScript functions.

	Set the `filter` and `take` properties. Don't set `partitionKey` or `rowKey`.


#### Storage tables example: Read a single table entity in C# or JavaScript

The queue message has the row key value and the table entity is read into a type that is user defined. The type includes `PartitionKey` and `RowKey`.

```csharp
public static void Run(string myQueueItem, Person personEntity, ILogger log)
{
    log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
    log.LogInformation($"Name in Person entity: {personEntity.Name}");
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

#### Storage tables example: C# example that reads multiple table entities based on a queue trigger

The C# code adds a reference to Newtonsoft JSON library so that we can utilize JSON constructs to process multiple table entries. The `queuePerson` argument is bound to a queue entry while the `inputTable` is bound to an Azure Storage Table. We can specify the `filter` property so that, for example, only table results for persons older than the person in the queue are returned ( specify `"Age gt {Age}"` where `{Age}` represents the age of the person from the queue). 

```csharp
#r "Newtonsoft.Json"
using System;

public static void Run(Person queuePerson, Newtonsoft.Json.Linq.JArray inputTable, ILogger log)
{
    foreach (Newtonsoft.Json.Linq.JToken jToken in inputTable)
    {
        Person p = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(jToken.ToString());
        log.LogInformation(p.ToString());
    }
}

public class Person 
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }

    public override string ToString() {
        return String.Format("Hi! My name is {0}. I am {1} years old.", Name, Age);
    }
}
``` 
