using Azure.Storage.Blobs;
using NETToolBox.BlobStorage.Abstractions;
using System.Collections.Concurrent;

namespace NetToolBox.BlobStorage.Azure
{
    public sealed class AzureBlobStorageFactory : IBlobStorageFactory
    {
        private readonly ConcurrentDictionary<string, BlobServiceClient> _blobServiceClientDictionary = new ConcurrentDictionary<string, BlobServiceClient>();
        private readonly ConcurrentDictionary<(string connectionString, string containerName), BlobContainerClient> _blobContainerClientDictionary = new ConcurrentDictionary<(string connectionString, string containerName), BlobContainerClient>();
        private readonly ConcurrentDictionary<(string connectionString, string containerName), IBlobStorage> _blobStorageDictionary = new ConcurrentDictionary<(string connectionString, string containerName), IBlobStorage>();
        public IBlobStorage GetBlobStorage(string connectionString, string containerName)
        {
            IBlobStorage retval;
            if (!_blobServiceClientDictionary.ContainsKey(connectionString))
            {
                _blobServiceClientDictionary.TryAdd(connectionString, new BlobServiceClient(connectionString));
            }

            if (!_blobContainerClientDictionary.ContainsKey((connectionString, containerName)))
            {
                var blobServiceClient = _blobServiceClientDictionary[connectionString];
                _blobContainerClientDictionary.TryAdd((connectionString, containerName), blobServiceClient.GetBlobContainerClient(containerName));
                _blobStorageDictionary.TryAdd((connectionString, containerName), new AzureBlobStorage(_blobContainerClientDictionary[(connectionString, containerName)]));
            }
            retval = _blobStorageDictionary[(connectionString, containerName)];
            return retval;
        }
    }
}
