using Azure.Identity;
using Azure.Storage.Blobs;
using NETToolBox.BlobStorage.Abstractions;
using System;
using System.Collections.Concurrent;

namespace NetToolBox.BlobStorage.Azure
{

    public sealed class AzureBlobStorageFactory : IBlobStorageFactory
    {
        private readonly ConcurrentDictionary<(string accountName, string containerName), BlobContainerClient> _blobContainerClientDictionary = new ConcurrentDictionary<(string accountName, string containerName), BlobContainerClient>();
        private readonly ConcurrentDictionary<(string accountName, string containerName), IBlobStorage> _blobStorageDictionary = new ConcurrentDictionary<(string accountName, string containerName), IBlobStorage>();
        public IBlobStorage GetBlobStorage(string accountName, string containerName)
        {
            IBlobStorage retval;


            if (!_blobContainerClientDictionary.ContainsKey((accountName, containerName)))
            {
                string containerEndpoint = string.Format("https://{0}.blob.core.windows.net/{1}",
                                                         accountName,
                                                         containerName);
                BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndpoint),
                                                                           new DefaultAzureCredential(true)); //call an interactive login until https://github.com/Azure/azure-sdk-for-net/issues/8658 is fixed
                containerClient.CreateIfNotExists();
                _blobContainerClientDictionary.TryAdd((accountName, containerName), containerClient);
                _blobStorageDictionary.TryAdd((accountName, containerName), new AzureBlobStorage(_blobContainerClientDictionary[(accountName, containerName)]));
            }
            retval = _blobStorageDictionary[(accountName, containerName)];
            return retval;
        }
    }
}
