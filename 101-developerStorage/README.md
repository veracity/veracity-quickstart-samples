# 101 Developer Storage 

The 101 Developer Storage sample is a sample that creates a management application. It creates a local blob on your machine for you to develop towards. When you then create your client application to access the blob, this will be similar to how you will interact with storage on Azure. If you would like to manage a storage in Azure, you may just modify the App.config accordingly.

The sample is written in C# and use the Azure Storage Client Library for .NET. The sample code does the following:

- Generate a shared access signature on a container
- Generate a shared access signature on a blob
- Create a stored access policy to manage signatures on a container's resources
- Test the shared access signatures in a client application

