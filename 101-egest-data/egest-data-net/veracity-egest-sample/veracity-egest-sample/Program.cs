using System;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;

namespace veracity_egest_sample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the constant holding the key provided by Veracity. Add your key.
            const string veracityContainerSAS = " < your SAS key go here >";

            // Add some methods under this line
            UseContainerSAS(veracityContainerSAS);
            // Hold the consol
            Console.ReadLine();

        }

        // We create a method that interact with the container
        static void UseContainerSAS(string sas)
        {
            // We try to performe operations with the SAS provided.

            //Return a reference to the container using the SAS URI.
            CloudBlobContainer container = new CloudBlobContainer(new Uri(sas));

            //Create a list to store blob URIs returned by a listing operation on the container.
            List<ICloudBlob> blobList = new List<ICloudBlob>();

            //Write operation: write a new blob to the container.
            try
            {
                CloudBlockBlob blob = container.GetBlockBlobReference("blobCreatedViaSAS.txt");
                string blobContent = "This Veracity blob was created with a shared access signature granting write permissions to the container.";
                blob.UploadText(blobContent);

                Console.WriteLine("We where able to write to a blob using this SAS key");
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Write operation failed using this SAS key");
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
                Console.WriteLine("List operation succeeded for SAS key");
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("List operation failed for this SAS key");
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
                    msRead.Position = 0;
                    // Just checking that we can read the buffer we retrieved
                    Console.WriteLine(System.Text.Encoding.UTF8.GetString(msRead.GetBuffer()));
                }
                Console.WriteLine("Read operation succeeded for SAS ");
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Read operation failed for SAS ");
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }
            Console.WriteLine();

            //Delete operation: Delete a blob in the container.
            try
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(blobList[0].Name);
                blob.Delete();
                Console.WriteLine("Delete operation succeeded for SAS ");
                Console.WriteLine();
            }
            catch (StorageException e)
            {
                Console.WriteLine("Delete operation failed for SAS ");
                Console.WriteLine("Additional error information: " + e.Message);
                Console.WriteLine();
            }
        }
    }
}
