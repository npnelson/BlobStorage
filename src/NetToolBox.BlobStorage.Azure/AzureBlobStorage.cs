using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NETToolBox.BlobStorage.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetToolBox.BlobStorage.Azure
{
    public sealed class AzureBlobStorage : IBlobStorage
    {
        private readonly BlobContainerClient _blobContainerClient;

        public AzureBlobStorage(BlobContainerClient blobContainerClient)
        {
            _blobContainerClient = blobContainerClient;
        }

        public async Task<List<string>> ListFilesAsync(string prefix, CancellationToken cancellationToken = default)
        {
            var retval = new List<string>();
            var blobResult = _blobContainerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, prefix, cancellationToken);
            await foreach (var result in blobResult.ConfigureAwait(false))
            {
                retval.Add(result.Name);
            }
            return retval;
        }

        public async Task<Stream> DownloadFileAsStreamAsync(string blobPath, CancellationToken cancellationToken = default)
        {
            var blob = _blobContainerClient.GetBlobClient(blobPath);
            BlobDownloadInfo download = await blob.DownloadAsync(cancellationToken).ConfigureAwait(false); //do not dispose download here with a using statement, it can dispose the stream you are returning
            return download.Content;
        }

        public async Task<string> DownloadFileAsTextAsync(string blobPath, CancellationToken cancellationToken = default)
        {
            string retval;
            var download = await DownloadFileAsStreamAsync(blobPath, cancellationToken).ConfigureAwait(false);
            using (var sr = new StreamReader(download))
            {
                retval = await sr.ReadToEndAsync().ConfigureAwait(false);
            }
            return retval;
        }

        public async Task<string> GetContentTypeAsync(string blobPath, CancellationToken cancellationToken = default)
        {
            var blob = _blobContainerClient.GetBlobClient(blobPath);
            var props = await blob.GetPropertiesAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            var retval = props.Value.ContentType;
            return retval;
        }

        public async Task<bool> IsHealthyAsync()
        {
            await _blobContainerClient.GetPropertiesAsync().ConfigureAwait(false); //we don't care about the properties now, if it comes to caring about the properties, we will have to create an abstraction for blobcontainerproperites
            return true;
        }

        public async Task StoreBlobAsBytesAsync(string blobPath, byte[] byteArray, CancellationToken cancellationToken = default)
        {
            Stream ms = new MemoryStream(byteArray);
            await StoreBlobAsStreamAsync(blobPath, ms, cancellationToken).ConfigureAwait(false);
        }

        public async Task StoreBlobAsStreamAsync(string blobPath, Stream stream, string contentType, CancellationToken cancellationToken = default)
        {
            var blob = _blobContainerClient.GetBlobClient(blobPath);
            await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken, conditions: null).ConfigureAwait(false);
        }

        public async Task StoreBlobAsStreamAsync(string blobPath, Stream stream, CancellationToken cancellationToken = default)
        {
            var blob = _blobContainerClient.GetBlobClient(blobPath);
            await blob.UploadAsync(stream, cancellationToken: cancellationToken, conditions: null).ConfigureAwait(false);
        }

        public async Task StoreBlobAsTextAsync(string blobPath, string blobContents, CancellationToken cancellationToken = default)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(blobContents);
            await StoreBlobAsBytesAsync(blobPath, byteArray, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(string blobPath, CancellationToken cancellationToken = default)
        {
            var blob = _blobContainerClient.GetBlobClient(blobPath);
            var response = await blob.ExistsAsync(cancellationToken).ConfigureAwait(false);
            return response.Value;
        }
    }
}