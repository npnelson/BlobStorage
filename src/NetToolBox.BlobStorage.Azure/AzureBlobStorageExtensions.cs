using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NETToolBox.BlobStorage.Abstractions;
using System.Collections.Generic;

namespace NetToolBox.BlobStorage.Azure
{
    public static class AzureBlobStorageExtensions
    {
        public static IServiceCollection AddAzureBlobStorageFactory(this IServiceCollection serviceCollection, string? tenant = null)
        {
            return serviceCollection.AddAzureBlobStorageFactoryInternal(null, tenant);
        }

        public static IServiceCollection AddAzureBlobStorageFactory(this IServiceCollection serviceCollection, List<(string accountName, string containerName)> containersToRegister, string? tenant = null)
        {
            return serviceCollection.AddAzureBlobStorageFactoryInternal(containersToRegister, tenant);
        }

        private static IServiceCollection AddAzureBlobStorageFactoryInternal(this IServiceCollection serviceCollection, List<(string accountName, string containerName)>? containersToRegister, string? tenant)
        {
            serviceCollection.TryAddSingleton<IBlobStorageFactory>(new AzureBlobStorageFactory(containersToRegister, tenant));
            return serviceCollection;
        }
    }
}