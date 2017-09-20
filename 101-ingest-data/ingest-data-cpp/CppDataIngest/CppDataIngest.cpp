#include "stdafx.h"
#include <was/storage_account.h>
#include <was/blob.h>

int main()
{
	//Return a reference to the container using the SAS URI.
	const utility::string_t veracity_container_sas(U("< your SAS key go here >"));
	azure::storage::cloud_blob_container container;
	azure::storage::cloud_block_blob blockBlob;
	try
	{
		container = azure::storage::cloud_blob_container::cloud_blob_container(azure::storage::storage_uri(veracity_container_sas));
		blockBlob = container.get_block_blob_reference(U("blobCreatedViaSAS.txt"));
	}
	catch (const std::exception e)
	{
		std::wcout << U("There was an error during blob referencing vis SAS: ") << e.what() << std::endl;
	}
	//Write operation: write a new blob to the container.
	try
	{
		blockBlob.upload_text(U("This Veracity blob was created with a shared access signature granting write permissions to the container."));
		std::wcout << U("We where able to write to a blob using this SAS key") << std::endl;
	}
	catch (const std::exception e)
	{
		std::wcout << U("There was an error during blob writing: ") << e.what() << std::endl;
	}

	//List operation: List the blobs in the container.
	try
	{
		azure::storage::list_blob_item_iterator end_of_results;
		for (auto i = container.list_blobs(); i != end_of_results; ++i)
		{
			if (i->is_blob())
			{
				std::wcout << U("Blob: ") << i->as_blob().uri().primary_uri().to_string() << std::endl;
			}
			else
			{
				// just a check if we have a directory in container
				std::wcout << U("Directory: ") << i->as_directory().uri().primary_uri().to_string() << std::endl;
			}
		}
		std::wcout << U("List operation succeeded for SAS key") << std::endl;
	}
	catch (const std::exception e)
	{
		std::wcout << U("There was an error during blob listing: ") << e.what() << std::endl;
	}

	//Read operation: Get a reference to one of the blobs in the container and read it.
	try
	{
		azure::storage::cloud_block_blob text_blob = container.get_block_blob_reference(U("blobCreatedViaSAS.txt"));
		utility::string_t text = text_blob.download_text();
		std::wcout << U("Read operation succeeded for SAS , content: ") << std::endl;
		std::wcout << text << std::endl;
	}
	catch (const std::exception e)
	{
		std::wcout << U("There was an error during blob downloading: ") << e.what() << std::endl;
	}

	//Delete operation: Delete a blob in the container.
	try
	{
		azure::storage::cloud_block_blob blockBlob = container.get_block_blob_reference(U("blobCreatedViaSAS.txt"));
		blockBlob.delete_blob();
		std::wcout << U("Delete blob operation succeeded for SAS");
	}
	catch (const std::exception e)
	{
		std::wcout << U("There was an error during blob deletion: ") << e.what() << std::endl;
	}

	std::string str;
	std::getline(std::cin, str);
    return 0;
}