using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NETToolBox.BlobStorage.Abstractions;
using System.Collections.Generic;

namespace NetToolBox.BlobStorage.Azure
{
    public static class AzureBlobStorageExtensions
    {
        public static IServiceCollection AddAzureBlobStorageFactory(this IServiceCollection serviceCollection)
        {

            return serviceCollection.AddAzureBlobStorageFactoryInternal(null);
        }

        public static IServiceCollection AddAzureBlobStorageFactory(this IServiceCollection serviceCollection, List<(string accountName, string containerName)> containersToRegister)
        {
            return serviceCollection.AddAzureBlobStorageFactoryInternal(containersToRegister);
        }

        private static IServiceCollection AddAzureBlobStorageFactoryInternal(this IServiceCollection serviceCollection, List<(string accountName, string containerName)>? containersToRegister)
        {
            serviceCollection.TryAddSingleton<IBlobStorageFactory>(new AzureBlobStorageFactory(containersToRegister));
            return serviceCollection;
        }

    }
}
