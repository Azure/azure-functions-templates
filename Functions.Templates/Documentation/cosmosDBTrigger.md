#### Settings for Azure Cosmos DB trigger binding

The Azure Cosmos DB Trigger leverages the [Azure Cosmos DB Change Feed](https://docs.microsoft.com/azure/cosmos-db/change-feed) to listen for changes across partitions. It uses a **second collection** to store *leases* over the partitions.

Both the collection being monitored for changes and the collection that will hold the leases need to be available for the trigger to work.

The settings for an Azure Cosmos DB trigger specifies the following properties and they can be set in either the portal or by using the `function.json` in the Advanced Editor with the corresponding property names:

- **Trigger type** or `type` : Must be set to *cosmosDBTrigger*.
- **Document collection parameter name** or `name` : The variable name used in function code for the list of documents. 
- **Trigger direction** or `direction` : Must be set to *in*. This parameter is automatically set if using the Azure Portal.
- **Azure Cosmos DB account connection** or `connectionStringSetting` : The name of an app setting that contains the connection string to the service which holds the collection to monitor.
- **Database name** or `databaseName` : The name of the database that holds the collection to monitor.
- **Collection name** or `collectionName` : The name of the collection to monitor.
- **Azure Cosmos DB account connection for leases** or `leaseConnectionStringSetting` : Optional. The name of an app setting that contains the connection string to the service which holds the lease collection. If not set it will connect to the service defined by `connectionStringSetting`. This parameter is automatically set if using the Azure Portal.
- **Database name for leases** or `leaseDatabaseName` : The name of the database that holds the collection to store leases. If not set, it will use the value of `databaseName`. This parameter is automatically set if using the Azure Portal.
- **Collection name for leases** or `leaseCollectionName` : The name of the collection to store leases. If not set, it will use "leases".
- **Create lease collection if it does not exist** or `createLeaseCollectionIfNotExists` : true/false. Checks for existence and automatically creates the leases collection. Default is `false`.
- **Collection throughput for leases** or `leaseCollectionThroughput` : When `createLeaseCollectionIfNotExists` is set to `true`, defines the amount of Request Units to assign to the created lease collection. This parameter is automatically set if using the Azure Portal.

> Connection strings used for the Lease collection require **write permission**.

The following settings customize the internal Change Feed mechanism and Lease collection usage, and can be set in the `function.json` in the Advanced Editor with the corresponding property names:

- `leaseCollectionPrefix` : When set, it adds a prefix to the leases created in the Lease collection for this Function, effectively allowing two separate Azure Functions to share the same Lease collection by using different prefixes.
- `feedPollDelay` : When set, it defines, in milliseconds, the delay in between polling a partition for new changes on the feed, after all current changes are drained. Default is 5000 (5 seconds).
- `leaseAcquireInterval` : When set, it defines, in milliseconds, the interval to kick off a task to compute if partitions are distributed evenly among known host instances. Default is 13000 (13 seconds).
- `leaseExpirationInterval` : When set, it defines, in milliseconds, the interval for which the lease is taken on a lease representing a partition. If the lease is not renewed within this interval, it will cause it to expire and ownership of the partition will move to another instance. Default is 60000 (60 seconds).
- `leaseRenewInterval` : When set, it defines, in milliseconds, the renew interval for all leases for partitions currently held by an instance. Default is 17000 (17 seconds).
- `checkpointFrequency` : When set, it defines, in milliseconds, the interval between lease checkpoints. Default is always after a successful Function call.
- `maxItemsPerInvocation` : When set, it customizes the maximum amount of items received per Function call.

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
