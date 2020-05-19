using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NETToolBox.BlobStorage.Abstractions;
using System.Collections.Generic;

namespace NetToolBox.BlobStorage.Azure
{
    public static class AzureBlobStorageExtensions
    {
        public static IServiceCollection AddBlobStorageFactory(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IBlobStorageFactory, AzureBlobStorageFactory>();
            return serviceCollection;
        }

        public static IServiceCollection AddBlobStorageFactory(this IServiceCollection serviceCollection, List<(string accountName, string containerName)> containersToRegister)
        {
            serviceCollection.TryAddSingleton<IBlobStorageFactory>(new AzureBlobStorageFactory(containersToRegister));
            return serviceCollection;
        }

    }
}
