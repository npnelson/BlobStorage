using Microsoft.Extensions.DependencyInjection;
using NetToolBox.BlobStorage.Azure;
using NETToolBox.BlobStorage.Abstractions;
using System;
using System.Text;
using System.Threading.Tasks;

namespace NetToolBox.BlobStorage.TestHarness
{
    static class Program
    {
        static async Task Main(string[] args)
        {

            var sp = new ServiceCollection();
            sp.AddAzureBlobStorageFactory(new System.Collections.Generic.List<(string accountName, string containerName)> { ("testStorage", "testContainer") });
            var sc = sp.BuildServiceProvider();
            //var blobFactory = new AzureBlobStorageFactory(new System.Collections.Generic.List<(string accountName, string containerName)> { ("testStorage", "testContainer") });
            var blobFactory = sc.GetRequiredService<IBlobStorageFactory>();

            var blobStorage = blobFactory.GetBlobStorage("validaccount", "testcontainer", false);
            var registrations = blobFactory.GetBlobStorageRegistrations();
            foreach (var container in registrations)
            {
                var retval = await blobFactory.GetBlobStorage(container.accountName, container.containerName).IsHealthyAsync();
            }
            string blobContents = DateTime.UtcNow.ToString();
            byte[] byteArray = Encoding.ASCII.GetBytes(blobContents);
            await blobStorage.StoreBlobAsBytesAsync("testblob.txt", byteArray);
            Console.WriteLine($"Wrote Blob {blobContents}");
        }


    }
}
