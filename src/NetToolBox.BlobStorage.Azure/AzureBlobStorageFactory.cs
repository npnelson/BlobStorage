using Azure.Identity;
using Azure.Storage.Blobs;
using NETToolBox.BlobStorage.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NetToolBox.BlobStorage.Azure
{

    public sealed class AzureBlobStorageFactory : IBlobStorageFactory
    {

        /// <summary>
        /// Use this constructor if you want to prime some containers at DI time. Useful for running health checks, especially in Azure Functions where each function instance might not have to refer to each container
        /// </summary>
        /// <param name="containersToRegister"></param>
        public AzureBlobStorageFactory(List<(string accountName, string containerName)>? containersToRegister)
        {
            if (containersToRegister != null)
            {
                foreach (var container in containersToRegister)
                {
                    GetBlobStorage(container.accountName, container.containerName, false); //we don't care about the return value, just need to add it           
                }
            }
        }
        private readonly ConcurrentDictionary<(string accountName, string containerName), BlobContainerClient> _blobContainerClientDictionary = new ConcurrentDictionary<(string accountName, string containerName), BlobContainerClient>();
        private readonly ConcurrentDictionary<(string accountName, string containerName), IBlobStorage> _blobStorageDictionary = new ConcurrentDictionary<(string accountName, string containerName), IBlobStorage>();
        public IBlobStorage GetBlobStorage(string accountName, string containerName, bool createContainerIfNotExists = true)
        {
            IBlobStorage retval;


            if (!_blobContainerClientDictionary.ContainsKey((accountName, containerName)))
            {
                BlobContainerClient containerClient;
                if (accountName.Equals("UseDevelopmentStorage=true", StringComparison.OrdinalIgnoreCase) || accountName.Contains("Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==", StringComparison.OrdinalIgnoreCase)) //when running in docker, it comes across as the expanded dev store account https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator
                {
                    containerClient = new BlobContainerClient("UseDevelopmentStorage=true", containerName);
                }
                else if (accountName.Contains("DefaultEndPoint", StringComparison.OrdinalIgnoreCase))
                {
                    containerClient = new BlobContainerClient(accountName, containerName);
                }
                else
                {
                    string containerEndpoint = string.Format("https://{0}.blob.core.windows.net/{1}",
                                                             accountName,
                                                             containerName);
                    containerClient = new BlobContainerClient(new Uri(containerEndpoint),
                                                                               new DefaultAzureCredential(true)); //call an interactive login until https://github.com/Azure/azure-sdk-for-net/issues/8658 is fixed
                }
                if (createContainerIfNotExists) containerClient.CreateIfNotExists();

                _blobContainerClientDictionary.TryAdd((accountName, containerName), containerClient);
                _blobStorageDictionary.TryAdd((accountName, containerName), new AzureBlobStorage(_blobContainerClientDictionary[(accountName, containerName)]));
            }
            retval = _blobStorageDictionary[(accountName, containerName)];
            return retval;
        }

        public List<(string accountName, string containerName)> GetBlobStorageRegistrations()
        {
            var retval = _blobContainerClientDictionary.Keys.ToList();
            return retval;
        }
    }
}
