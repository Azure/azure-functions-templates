// An HTTP trigger Azure Function that returns a SAS token for Azure Storage for the specified container. 
// You can also optionally specify a particular blob name and access permissions. 
// To learn more, see https://github.com/Azure/azure-webjobs-sdk-templates/blob/master/Templates/SasToken-CSharp/readme.md

// Request body format:
// - `ContainerName` - *required*. Name of container in storage account
// - `BlobName` - *optional*. Used to scope permissions to a particular blob
// - `Permission` - *optional*. Default value is read permissions. The format matches the enum values of SharedAccessBlobPermissions. 
//    Possible values are "Read", "Write", "Delete", "List", "Add", "Create". Comma-separate multiple permissions, such as "Read, Write, Create".
#if (portalTemplates)
#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

public static HttpResponseMessage Run(Input input, CloudBlobDirectory blobDirectory, TraceWriter log)
{
#endif
#if (vsTemplates)
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Company.Function
{
    public static class SasTokenCSharp
    {
        [FunctionName("FunctionNameValue")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.AuthLevelValue, "post")]Input input, [Blob("PathValue", FileAccess.Read, Connection = "ConnectionValue")]CloudBlobDirectory blobDirectory, TraceWriter log)
#endif
        {
            var permissions = SharedAccessBlobPermissions.Read; // default to read permissions

            // if permission was supplied, check if it is a possible value
            if (!string.IsNullOrWhiteSpace(input.Permission))
            {
                if (!Enum.TryParse(input.Permission, out permissions))
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Invalid value for 'permissions'") };
                }
            }

            var container = blobDirectory.Container;
            var sasToken = input.BlobName != null ?
                 GetBlobSasToken(container, input.BlobName, permissions) :
                 GetContainerSasToken(container, permissions);

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(sasToken) };
        }

        public class Input
        {
            public string ContainerName { get; set; }
            public string BlobName { get; set; }
            public string Permission { get; set; }
        }

        public static string GetBlobSasToken(CloudBlobContainer container, string blobName, SharedAccessBlobPermissions permissions, string policyName = null)
        {
            string sasBlobToken;

            // Get a reference to a blob within the container.
            // Note that the blob may not exist yet, but a SAS can still be created for it.
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (policyName == null)
            {
                var adHocSas = CreateAdHocSasPolicy(permissions);

                // Generate the shared access signature on the blob, setting the constraints directly on the signature.
                sasBlobToken = blob.GetSharedAccessSignature(adHocSas);
            }
            else
            {
                // Generate the shared access signature on the blob. In this case, all of the constraints for the
                // shared access signature are specified on the container's stored access policy.
                sasBlobToken = blob.GetSharedAccessSignature(null, policyName);
            }

            return sasBlobToken;
        }

        public static string GetContainerSasToken(CloudBlobContainer container, SharedAccessBlobPermissions permissions, string storedPolicyName = null)
        {
            string sasContainerToken;

            // If no stored policy is specified, create a new access policy and define its constraints.
            if (storedPolicyName == null)
            {
                var adHocSas = CreateAdHocSasPolicy(permissions);

                // Generate the shared access signature on the container, setting the constraints directly on the signature.
                sasContainerToken = container.GetSharedAccessSignature(adHocSas, null);
            }
            else
            {
                // Generate the shared access signature on the container. In this case, all of the constraints for the
                // shared access signature are specified on the stored access policy, which is provided by name.
                // It is also possible to specify some constraints on an ad-hoc SAS and others on the stored access policy.
                // However, a constraint must be specified on one or the other; it cannot be specified on both.
                sasContainerToken = container.GetSharedAccessSignature(null, storedPolicyName);
            }

            return sasContainerToken;
        }

        private static SharedAccessBlobPolicy CreateAdHocSasPolicy(SharedAccessBlobPermissions permissions)
        {
            // Create a new access policy and define its constraints.
            // Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad-hoc SAS, and 
            // to construct a shared access policy that is saved to the container's shared access policies. 
            return new SharedAccessBlobPolicy()
            {
                // Set start time to five minutes before now to avoid clock skew.
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),
                Permissions = permissions
            };
        }
#if (vsTemplates)
    }
}
#endif