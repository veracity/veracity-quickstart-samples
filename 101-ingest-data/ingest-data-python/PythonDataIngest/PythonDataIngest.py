from azure.storage.blob import BlockBlobService

# not sure about accountName, seems to be required.
accountName = "<storage account name>"
veracityContainerSAS = "< your sas key without question mark'?' >"
containerName = "< container name >"

# create service and keep reference to SAS container
print("Creating SAS service with {0} account".format(accountName))
try:
    sas_service = BlockBlobService(account_name=accountName, sas_token=veracityContainerSAS)
except Exception as e:
    print("There was an error during SAS service creation. Details: {0}".format(e))

# upload text blob to container
blobName = "blobCreatedViaSAS.txt"
print("Uploading {0} blob to {1} container...".format(blobName, containerName))
try:
    sas_service.create_blob_from_text(containerName, blobName, 'This Veracity blob was created with a shared access signature granting write permissions to the container.')
except Exception as e:
    print("There was an error during blob uploading. Details: {0}".format(e))

# list blobs in container
print("Blobs in container: ")
try:
    generator = sas_service.list_blobs(containerName)
    for blob in generator:
        print(blob.name)
except Exception as e:
    print("There was an error during blobs listing. Details: {0}".format(e))

# read blob from container
print("")
print("{0} blob content: ".format(blobName))
try:
    blob_text = sas_service.get_blob_to_text(containerName, blobName)
    print(blob_text.content)
except Exception as e:
    print("There was an error during blob reading. Details: {0}".format(e))

# delete blob
print("")
print("Deleting {0} blob...".format(blobName))
try:
    sas_service.delete_blob(containerName, blobName)
except Exception as e:
    print("There was an error during deletion of blob. Details: {0}".format(e))

print("Blob {0} deleted.".format(blobName))