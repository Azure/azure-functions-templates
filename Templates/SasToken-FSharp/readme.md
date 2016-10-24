# SAS Token Generator - F<span>#</span>

An HTTP trigger Azure Function that returns a SAS token for Azure Storage for the specified container, blob, and permissions. A SAS token provides a secure way for client apps to access particular storage account resources, without giving them the full control of the storage access key.

##Calling the function

To request a SAS token, send an HTTP post to your function URI, including the API key if you've specified one. The request body format is:

- `container` - *required*. Name of container in storage account
- `blobName` - *optional*. Used to scope permissions to a particular blob
- `permissions` - *optional*. Default value is read permissions. The format matches the enum values of SharedAccessBlobPermissions. Possible values are "Read", "Write", "Delete", "List", "Add", "Create". Comma-separate multiple permissions, such as "Read, Write, Create".

## How it works

When you create a storage account, you get two storage access keys, which provide full control over the storage account contents. Since these keys are admin credentials, they should **never** be distributed with a client app. 

Instead, clients should use a shared access signature (SAS) for delegated access to storage resources. A SAS token, which is appended to a storage resource URI, provides access to only a particular resource for a limited period of time. A SAS token can be scoped to a blob or a container and specifies access permissions (such as read or write).

A SAS token is usually generated server-side, using the account access key and the Azure Storage SDK. This sample shows how to use an Azure Function as a SAS token service. Web and mobile clients can call this function to request access to a particular container or blob. By default, the sample creates a token that expires after an hour, but this can be customized.

If the function will be called from a mobile client or a JavaScript web app, we recommend that you add authentication to your Function using [App Service Authentication/Authorization](https://azure.microsoft.com/en-us/documentation/articles/app-service-authentication-overview/). The API key is usually insufficent for security purposes since it can be discovered by sniffing traffic or decompiling the client app.

## Learn more

- [Authentication and authorization in Azure App Service](https://azure.microsoft.com/en-us/documentation/articles/app-service-authentication-overview/)
- [Shared Access Signatures: Understanding the SAS Model](https://azure.microsoft.com/documentation/articles/storage-dotnet-shared-access-signature-part-1/)
- [Create and use a SAS with Blob storage](https://azure.microsoft.com/documentation/articles/storage-dotnet-shared-access-signature-part-2/)
- [Delegating Access with a Shared Access Signature](https://msdn.microsoft.com/library/ee395415.aspx)
- [Establishing a Stored Access Policy](https://msdn.microsoft.com/library/dn140257.aspx)
