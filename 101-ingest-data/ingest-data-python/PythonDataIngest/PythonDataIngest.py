from azure.storage.blob import BlockBlobService
from azure.storage.blob import ContentSettings

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


# upload a file to blob
blobName = "NameAtDestination.csv"
localName= 'sensorData.txt'

try:
    sas_service.create_blob_from_path(
       containerName,
       blobName,
       localName,
       content_settings=ContentSettings(content_type='sensor/csv')
    )
except Exception as e:
    print("There was an error during blob uploading. Details: {0}".format(e))

    
# upload text blob to container
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
