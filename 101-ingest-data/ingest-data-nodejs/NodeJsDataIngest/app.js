'use strict';
var azure = require('azure-storage');
var hostUri = "https://<name>.blob.core.windows.net";
var blobSas = "< your SAS key go here >";
var containerName = "< container name >";
var blobName = "blobCreatedViaSAS.txt";
var sharedBlobSvc = azure.createBlobServiceWithSas(hostUri, blobSas);

var text = "This Veracity blob was created with a shared access signature granting write permissions to the container.";

performAzureOperations();

function performAzureOperations() {

    // writing test to blob
    sharedBlobSvc.createAppendBlobFromText(
        containerName,
        blobName,
        text,
        function(error, result, response) {
            if (error) {
                console.log("There was an error while doing blob upload.");
                console.error(error);
            } else {
                console.log("We where able to write to a blob using this SAS key");

                // listing blobs in container
                sharedBlobSvc.listBlobsSegmented(
                    containerName,
                    null,
                    function(error, result, response) {
                        if (error) {
                            console.log("There was an error during listing blobs in container %s", containerName);
                            console.error(error);
                        } else {
                            console.log('%s blobs: ', containerName);
                            var index;
                            for (index = 0; index < result.entries.length; index++) {
                                console.log(result.entries[index].name);
                            }
                            console.log("We where able to write list blobs using SAS key");

                            // downloading blob to text
                            sharedBlobSvc.getBlobToText(
                                containerName,
                                blobName,
                                function(error, blobContent, blob) {
                                    if (error) {
                                        console.error("Couldn't download blob %s", blobName);
                                        console.error(error);
                                    } else {
                                        console.log("Sucessfully downloaded blob %s", blobName);
                                        console.log(blobContent);

                                        // deleting a blob
                                        sharedBlobSvc.deleteBlob(containerName, blobName, function (error, response) {
                                            if (error) {
                                                console.log("There was an error during deletion of blob  %s", blobName);
                                                console.error(error);
                                            } else
                                                console.log("%s blob deleted sucessfully", blobName);
                                        });
                                    }
                                });
                        }
                    });

            }
        });
}