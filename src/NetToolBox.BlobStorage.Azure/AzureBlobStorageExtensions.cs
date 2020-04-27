using Microsoft.Extensions.DependencyInjection;
using NETToolBox.BlobStorage.Abstractions;

namespace NetToolBox.BlobStorage.Azure
{
    public static class AzureBlobStorageExtensions
    {
        public static IServiceCollection AddBlobStorageFactory(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IBlobStorageFactory, AzureBlobStorageFactory>();
            return serviceCollection;
        }
    }
}
