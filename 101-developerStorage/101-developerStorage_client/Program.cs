using System;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Text;

namespace _101_developerStorage_client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Update the containerSAS, blobSAS, containerSASWithAccessPolicy and blobSASWithAccessPolicy
            const string containerSAS = "http://127.0.0.1:10000/devstoreaccount1/veracitycontainer?sv=2017-04-17&sr=c&sig=2COkOwtsKb%2BmcRugdr%2FzJhS9vEmxhiBH%2FdnxHU8NNQg%3D&se=2017-08-24T18%3A58%3A31Z&sp=wl";   // "<your container SAS>";
            const string blobSAS = "http://127.0.0.1:10000/devstoreaccount1/veracitycontainer/veracityblob.txt?sv=2017-04-17&sr=b&sig=qPj%2F2q3vWATuNRG1dOIi8bFFc09BghDGcCRl2AycjEM%3D&st=2017-08-23T18%3A53%3A31Z&se=2017-08-24T18%3A58%3A31Z&sp=rw";// " < your blob SAS>";
            const string containerSASWithAccessPolicy = "http://127.0.0.1:10000/devstoreaccount1/veracitycontainer?sv=2017-04-17&sr=c&si=veracitytutorialpolicy&sig=vGFHRS3DCOJeRD%2FtZnCoEuZ97xW9I2QzxpMVbX%2FThWo%3D";     // " < your container SAS with access policy>";
            const string blobSASWithAccessPolicy = "http://127.0.0.1:10000/devstoreaccount1/veracitycontainer/sasblobpolicy.txt?sv=2017-04-17&sr=b&si=veracitytutorialpolicy&sig=Dg6lccuhKcGmFe7HxFqhQ7aHzdt6RjWfnb4ZoI%2Bamw8%3D";      // " < your blob SAS with access policy>";


            // We call the test methods with the shared access signatures created on the container, with and without the access policy.
            UseContainerSAS(containerSAS);
            UseContainerSAS(containerSASWithAccessPolicy);

            // We create some test methods with the shared access signatures created on the blob, with and without the access policy.
            UseBlobSAS(blobSAS);
            UseBlobSAS(blobSASWithAccessPolicy);

            Console.ReadLine();
        }


        // We create a method that interact with the container
        static void UseContainerSAS(string sas)
        {
            //Try performing container operations with the SAS provided.

            //Return a reference to the container using the SAS URI.
            CloudBlobContainer container = new CloudBlobContainer(new Uri(sas));

            //Create a list to store blob URIs returned by a listing operation on the container.
            List<ICloudBlob> blobList = new List<ICloudBlob>();

            //Write operation: write a new blob to the container.
            try
            {
                CloudBlockBlob blob = container.GetBlockBlobReference("blobCreatedViaSAS.txt");
                string blobContent = "This blob was created with a shared access signature granting write permissions to the container. ";
                blob.UploadText(blobContent);

                Console.WriteLine("Write operation succeeded for SAS " + sas);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Write operation failed for SAS " + sas);
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }

            //List operation: List the blobs in the container.
            try
            {
                foreach (ICloudBlob blob in container.ListBlobs())
                {
                    blobList.Add(blob);
                }
                Console.WriteLine("List operation succeeded for SAS " + sas);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("List operation failed for SAS " + sas);
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }

            //Read operation: Get a reference to one of the blobs in the container and read it.
            try
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(blobList[0].Name);
                MemoryStream msRead = new MemoryStream();
                msRead.Position = 0;
                using (msRead)
                {
                    blob.DownloadToStream(msRead);
                    Console.WriteLine(msRead.Length);
                }
                Console.WriteLine("Read operation succeeded for SAS " + sas);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Read operation failed for SAS " + sas);
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }
            Console.WriteLine();

            //Delete operation: Delete a blob in the container.
            try
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(blobList[0].Name);
                blob.Delete();
                Console.WriteLine("Delete operation succeeded for SAS " + sas);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Delete operation failed for SAS " + sas);
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }
        }

        // We create a test method to interact with the blobs
        static void UseBlobSAS(string sas)
        {
            //Try performing blob operations using the SAS provided.

            //Return a reference to the blob using the SAS URI.
            CloudBlockBlob blob = new CloudBlockBlob(new Uri(sas));

            //Write operation: Write a new blob to the container.
            try
            {
                string blobContent = "This blob was created with a shared access signature granting write permissions to the blob. ";
                MemoryStream msWrite = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
                msWrite.Position = 0;
                using (msWrite)
                {
                    blob.UploadFromStream(msWrite);
                }
                Console.WriteLine("Write operation succeeded for SAS " + sas);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Write operation failed for SAS " + sas);
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }

            //Read operation: Read the contents of the blob.
            try
            {
                MemoryStream msRead = new MemoryStream();
                using (msRead)
                {
                    blob.DownloadToStream(msRead);
                    msRead.Position = 0;
                    using (StreamReader reader = new StreamReader(msRead, true))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                        }
                    }
                }
                Console.WriteLine("Read operation succeeded for SAS " + sas);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Read operation failed for SAS " + sas);
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }

            //Delete operation: Delete the blob.
            try
            {
                blob.Delete();
                Console.WriteLine("Delete operation succeeded for SAS " + sas);
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Delete operation failed for SAS " + sas);
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }
        }
    }
}
