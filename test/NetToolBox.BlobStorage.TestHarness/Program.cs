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
            var blobFactory = new AzureBlobStorageFactory();
            var blobStorage = blobFactory.GetBlobStorage("mohpreferenceblobstorage", "testcontainer");
            string blobContents = DateTime.UtcNow.ToString();
            byte[] byteArray = Encoding.ASCII.GetBytes(blobContents);
            await blobStorage.StoreBlobAsBytesAsync("testblob.txt", byteArray);
            Console.WriteLine($"Wrote Blob {blobContents}");
        }


    }
}
