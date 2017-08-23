using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Text;

namespace _101_developerStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            //Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to a container to use for the sample code, and create it if it does not exist.
            CloudBlobContainer container = blobClient.GetContainerReference("veracitycontainer");
            container.CreateIfNotExists();

            //Generate a SAS URI for the container, without a stored access policy.
            Console.WriteLine("Container SAS URI: " + GetContainerSasUri(container));
            Console.WriteLine();

            //Generate a SAS URI for a blob within the container, without a stored access policy.
            Console.WriteLine("Blob SAS URI: " + GetBlobSasUri(container));
            Console.WriteLine();

            //Clear any existing access policies on container.
            BlobContainerPermissions perms = container.GetPermissions();
            perms.SharedAccessPolicies.Clear();
            container.SetPermissions(perms);

            //Create a new access policy on the container, which may be optionally used to provide constraints for
            //shared access signatures on the container and the blob.
            string sharedAccessPolicyName = "veracitytutorialpolicy";
            CreateSharedAccessPolicy(blobClient, container, sharedAccessPolicyName);

            //Generate a SAS URI for the container, using a stored access policy to set constraints on the SAS.
            Console.WriteLine("Container SAS URI using stored access policy: " + GetContainerSasUriWithPolicy(container, sharedAccessPolicyName));
            Console.WriteLine();

            //Generate a SAS URI for a blob within the container, using a stored access policy to set constraints on the SAS.
            Console.WriteLine("Blob SAS URI using stored access policy: " + GetBlobSasUriWithPolicy(container, sharedAccessPolicyName));
            Console.WriteLine();

            Console.ReadLine();
        }

        // Lets create a method that will create the SAS URI for the container
        static string GetContainerSasUri(CloudBlobContainer container)
        {
            //Set the expiry time and permissions for the container.
            //In this case no start time is specified, so the shared access signature becomes valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24);
            sasConstraints.Permissions = SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Write;

            //Generate the shared access signature on the container, setting the constraints directly on the signature.
            string sasContainerToken = container.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return container.Uri + sasContainerToken;
        }

        // Lets create the method that will create the SAS URI for a Blob
        static string GetBlobSasUri(CloudBlobContainer container)
        {
            //Get a reference to a blob within the container.
            CloudBlockBlob blob = container.GetBlockBlobReference("veracityblob.txt");

            //Upload text to the blob. If the blob does not yet exist, it will be created.
            //If the blob does exist, its existing content will be overwritten.
            string blobContent = "This blob will be accessible to clients via a shared access signature (SAS). This is similar to how the clients will read data from Veracity.";
            blob.UploadText(blobContent);

            //Set the expiry time and permissions for the blob.
            //In this case, the start time is specified as a few minutes in the past, to mitigate clock skew.
            //The shared access signature will be valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;

            //Generate the shared access signature on the blob, setting the constraints directly on the signature.
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
        }


        // Lets create a method that enables us to modify the SAS policy to the container. We allow read, write and list
        static void CreateSharedAccessPolicy(CloudBlobClient blobClient, CloudBlobContainer container, string policyName)
        {
            //Get the container's existing permissions.
            BlobContainerPermissions permissions = container.GetPermissions();

            //Create a new shared access policy and define its constraints.
            SharedAccessBlobPolicy sharedPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24),
                Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read
            };

            //Add the new policy to the container's permissions, and set the container's permissions.
            permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
            container.SetPermissions(permissions);
        }

        // Almost done. Just need this method to associate the signature with the stored access policy we created
        static string GetContainerSasUriWithPolicy(CloudBlobContainer container, string policyName)
        {
            //Generate the shared access signature on the container. In this case, all of the constraints for the
            //shared access signature are specified on the stored access policy.
            string sasContainerToken = container.GetSharedAccessSignature(null, policyName);

            //Return the URI string for the container, including the SAS token.
            return container.Uri + sasContainerToken;
        }

        // Finaly we need a method to create another blob and generate a shared access signature that's associated with a stored access policy.
        static string GetBlobSasUriWithPolicy(CloudBlobContainer container, string policyName)
        {
            //Get a reference to a blob within the container.
            CloudBlockBlob blob = container.GetBlockBlobReference("sasblobpolicy.txt");

            //Upload text to the blob. If the blob does not yet exist, it will be created.
            //If the blob does exist, its existing content will be overwritten.
            string blobContent = "This blob will be accessible to clients via a shared access signature. " +
            "A stored access policy defines the constraints for the signature.";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
            ms.Position = 0;
            using (ms)
            {
                blob.UploadFromStream(ms);
            }

            //Generate the shared access signature on the blob.
            string sasBlobToken = blob.GetSharedAccessSignature(null, policyName);

            //Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
        }
    }
}
