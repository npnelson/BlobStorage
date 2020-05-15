using NetToolBox.BlobStorage.Azure;
using System;
using System.Text;
using System.Threading.Tasks;

namespace NetToolBox.BlobStorage.TestHarness
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            //var blobFactory = new AzureBlobStorageFactory(new System.Collections.Generic.List<(string accountName, string containerName)> { ("testStorage", "testContainer") });
            var blobFactory = new AzureBlobStorageFactory();

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
