#### Settings for Cosmos DB trigger binding

The Cosmos DB Trigger leverages the [Cosmos DB Change Feed](https://docs.microsoft.com/azure/cosmos-db/change-feed) to listen for changes across partitions. It uses a **second collection** to store *leases* over the partitions.

Both the collection being monitored for changes and the collection that will hold the leases need to be available for the trigger to work.

The settings for an Azure Cosmos DB trigger specifies the following properties:

- `type` : Must be set to *cosmosDBTrigger*.
- `name` : The variable name used in function code for the list of documents. 
- `direction` : Must be set to *in*. 
- `connectionStringSetting` : The name of an app setting that contains the connection string to the service which holds the collection to monitor.
- `databaseName` : The name of the database that holds the collection to monitor.
- `collectionName` : The name of the collection to monitor.
- `leaseConnectionStringSetting` : Optional. The name of an app setting that contains the connection string to the service which holds the lease collection. If not set it will connect to the service defined by `connectionStringSetting`.
- `leaseDatabaseName` : The name of the database that holds the collection to store leases. If not set, it will use the value of `databaseName`.
- `leaseCollectionName` : The name of the collection to store leases. If not set, it will use "leases".
- `createLeaseCollectionIfNotExists` : true/false. Checks for existence and automatically creates the leases collection. Default is `false`.
- `leaseCollectionThroughput` : When `createLeaseCollectionIfNotExists` is set to `true`, defines the amount of Request Units to assign to the created lease collection.

> Connection strings used for the Lease collection require **write permission**.

#### Azure Cosmos DB trigger C# example
 
	#r "Microsoft.Azure.Documents.Client"
	using Microsoft.Azure.Documents;
	using System.Collections.Generic;
	using System;
	public static void Run(IReadOnlyList<Document> input, TraceWriter log)
	{
		log.Verbose("Documents modified " + input.Count);
		log.Verbose("First document Id " + input[0].Id);
	}

#### Azure Cosmos DB trigger JavaScript example

	module.exports = function (context, input) {
		context.log('First document Id modified : ', input[0].id);

		context.done();
	}
