using Microsoft.Extensions.DependencyInjection;
using NETToolBox.BlobStorage.Abstractions;
using System.Collections.Generic;

namespace NetToolBox.BlobStorage.Azure
{
    public static class AzureBlobStorageExtensions
    {
        public static IServiceCollection AddBlobStorageFactory(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IBlobStorageFactory, AzureBlobStorageFactory>();
            return serviceCollection;
        }

        public static IServiceCollection AddBlobStorageFactory(this IServiceCollection serviceCollection, List<(string accountName, string containerName)> containersToRegister)
        {
            serviceCollection.AddSingleton<IBlobStorageFactory>(new AzureBlobStorageFactory(containersToRegister));
            return serviceCollection;
        }

    }
}
