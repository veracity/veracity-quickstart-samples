import java.net.URI;
import java.util.ArrayList;

import com.microsoft.azure.storage.*;
import com.microsoft.azure.storage.blob.*;

public class DataEgest {

	public static void main(String[] args) {
		try {
			final String veracityContainerSAS = "< your SAS key go here >";

            // Container name must be lower case.
            CloudBlobContainer container = new CloudBlobContainer(new StorageUri(new URI(veracityContainerSAS)));

            //Write operation: write a new blob to the container.
            CloudBlockBlob blob = container.getBlockBlobReference("blobCreatedViaSAS.txt");
            String blobContent = "This Veracity blob was created with a shared access signature granting write permissions to the container.";
            blob.uploadText(blobContent);

            System.out.println("We where able to write to a blob using this SAS key");
            System.out.println();

            //Create a list to store blob URIs returned by a listing operation on the container.
            ArrayList<CloudBlob> blobList = new ArrayList<CloudBlob>();
            for (ListBlobItem blobItem : container.listBlobs()) {
            	if (blobItem instanceof CloudBlob) {
            		blobList.add((CloudBlob)blobItem);
            	}
            }
            System.out.println("List operation succeeded for SAS key");
            System.out.println();

            //Read operation: Get a reference to one of the blobs in the container and read it.
            CloudBlockBlob blockBlob = container.getBlockBlobReference(blobList.get(0).getName());
            String content = blockBlob.downloadText("UTF-8", null, null, null);
            System.out.println("Read operation succeeded for SAS ");
            System.out.println("Content: " + content);
            System.out.println();
            
            //Delete operation: Delete a blob in the container.
            CloudBlockBlob blockBlobToDelete = container.getBlockBlobReference(blobList.get(0).getName());
            blockBlobToDelete.delete();
            System.out.println("Delete operation succeeded for SAS ");
        }
        catch (StorageException storageException) {
            System.out.print("StorageException encountered: ");
            System.out.println(storageException.getMessage());
            System.exit(-1);
        }
        catch (Exception e) {
            System.out.print("Exception encountered: ");
            System.out.println(e.getMessage());
            System.exit(-1);
        }
	}
}
