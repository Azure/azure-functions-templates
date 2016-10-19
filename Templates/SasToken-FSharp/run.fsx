// An HTTP trigger Azure Function that returns a SAS token for Azure Storage for the specified container. 
// You can also optionally specify a particular blob name and access permissions. 
// To learn more, see https://github.com/Azure/azure-webjobs-sdk-templates/blob/master/Templates/SasToken-FSharp/readme.md

#r "System.Configuration"
#r "System.Net.Http"
#r "Microsoft.WindowsAzure.Storage"

open System
open System.Net
open System.Net.Http
open System.Configuration
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Blob
open FSharp.Interop.Dynamic

// Request body format: 
// - `container` - *required*. Name of container in storage account
// - `blobName` - *optional*. Used to scope permissions to a particular blob
// - `permissions` - *optional*. Default value is read permissions. The format matches the enum values of SharedAccessBlobPermissions. 
//    Possible values are "Read", "Write", "Delete", "List", "Add", "Create". Comma-separate multiple permissions, such as "Read, Write, Create".

type Response = {
    Token: string
    Uri: string
}

let CreateAdHocSasPolicy permissions =
    let adHocSas = SharedAccessBlobPolicy()
    adHocSas.SharedAccessStartTime <- DateTimeOffset.UtcNow.AddMinutes(-5.) |> Nullable
    adHocSas.SharedAccessExpiryTime <- DateTimeOffset.UtcNow.AddHours(1.) |> Nullable
    adHocSas.Permissions <- permissions
    adHocSas

let GetContainerSasToken (container: CloudBlobContainer) permissions =
    let adHocSas = CreateAdHocSasPolicy permissions
    container.GetSharedAccessSignature(adHocSas, null)

let GetBlobSasToken (container: CloudBlobContainer) blobName permissions =
    let blob = container.GetBlockBlobReference(blobName)
    let adHocSas = CreateAdHocSasPolicy permissions
    blob.GetSharedAccessSignature(adHocSas)

let Run(req: HttpRequestMessage, log: TraceWriter) =
    async {
        let! data = req.Content.ReadAsAsync<obj>() |> Async.AwaitTask
        let container = data?container

        if isNull container then
            return req.CreateResponse(HttpStatusCode.BadRequest, "Specify value for container")

        else
        let mutable permissions = SharedAccessBlobPermissions.Read

        if not (Enum.TryParse(data?permissions.ToString(), &permissions)) then
            return req.CreateResponse(HttpStatusCode.BadRequest, "Invalid value for 'permissions'")

        else
        let storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Item("AzureWebJobsStorage"))
        let blobClient = storageAccount.CreateCloudBlobClient()
        let container = blobClient.GetContainerReference(container.ToString())
        let blobName = data?blobName
        let sasToken =
            if isNull blobName then
                GetContainerSasToken container permissions
            else
                GetBlobSasToken container blobName permissions

        return req.CreateResponse(HttpStatusCode.OK, { Token = sasToken; Uri = container.Uri.ToString() + sasToken })
    } |> Async.RunSynchronously
