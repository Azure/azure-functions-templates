## Todo List with Node.js via HttpTrigger & Storage Table
HttpTrigger allows you to implement CRUD implementation over Rest API. This template implements a simple HTTP CRUD API over an Azure Storage Table, allowing todo item entities to be created and read.

## How it works
The http methods are mapped to CRUD operations on an Azure Storage Table binding. `GET` in this function is mapped to `Read all entities` and `POST` is mapped to `Create a new entity`. Http POST expects JSON data as shown below.
```JSON
{
    "item": "todo item",
    "completed": false
}
```

## Limitations
Currently modifying/deleting an existing entity through Table Storage binding is not supported. Hence this template is limited to Read and Create operations only.
