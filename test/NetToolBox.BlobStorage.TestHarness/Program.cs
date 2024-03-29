﻿using Microsoft.Extensions.DependencyInjection;
using NetToolBox.BlobStorage.Azure;
using NETToolBox.BlobStorage.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NetToolBox.BlobStorage.TestHarness
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var sp = new ServiceCollection();
            //sp.AddAzureBlobStorageFactory(new System.Collections.Generic.List<(string accountName, string containerName)> { ("testStorage", "testContainer") });
            sp.AddAzureBlobStorageFactory("");
            var sc = sp.BuildServiceProvider();
            //var blobFactory = new AzureBlobStorageFactory(new System.Collections.Generic.List<(string accountName, string containerName)> { ("testStorage", "testContainer") });
            var blobFactory = sc.GetRequiredService<IBlobStorageFactory>();

            var blobStorage = blobFactory.GetBlobStorage("", "");
            //var registrations = blobFactory.GetBlobStorageRegistrations();
            //foreach (var container in registrations)
            //{
            //    var retval = await blobFactory.GetBlobStorage(container.accountName, container.containerName).IsHealthyAsync();
            //}
            string blobContents = DateTime.UtcNow.ToString();
            var stream = File.OpenRead("");
            await blobStorage.StoreBlobAsStreamAsync("", stream, "");

            Console.WriteLine($"Wrote Blob {blobContents}");
            var contentType = await blobStorage.GetContentTypeAsync("");
            Console.ReadLine();
        }
    }
}