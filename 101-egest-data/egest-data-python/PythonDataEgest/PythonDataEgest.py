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

# list blobs in container
print("Blobs in container: ")
try:
    generator = sas_service.list_blobs(containerName)
    for blob in generator:
        print(blob.name)
except Exception as e:
    print("There was an error during blobs listing. Details: {0}".format(e))

# download blob to blob
print("Download blob to path")
try:
    sas_service.get_blob_to_path(containerName, blobName, 'downloadedName.csv')
except Exception as e:
    print("There was an error during blob download. Details: {0}".format(e))


# read text from blob
print("")
print("{0} blob content: ".format(blobName))
try:
    blob_text = sas_service.get_blob_to_text(containerName, blobName)
    print(blob_text.content)
except Exception as e:
    print("There was an error during blob reading. Details: {0}".format(e))
