using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NETToolBox.BlobStorage.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace NetToolBox.BlobStorage.Azure
{
    public sealed class AzureBlobStorage : IBlobStorage
    {
        private readonly BlobContainerClient _blobContainerClient;

        internal AzureBlobStorage(BlobContainerClient blobContainerClient)
        {
            _blobContainerClient = blobContainerClient;
        }

        public async Task<Stream> DownloadFileAsStreamAsync(string blobPath)
        {

            var blob = _blobContainerClient.GetBlobClient(blobPath);
            using (BlobDownloadInfo download = await blob.DownloadAsync().ConfigureAwait(false))
            {
                return download.Content;
            }
        }

        public async Task<string> DownloadFileAsTextAsync(string blobPath)
        {
            string retval;
            var download = await DownloadFileAsStreamAsync(blobPath).ConfigureAwait(false);
            using (var sr = new StreamReader(download))
            {
                retval = await sr.ReadToEndAsync().ConfigureAwait(false);
            }
            return retval;
        }

        public async
            Task StoreBlobAsBytesAsync(string blobPath, byte[] byteArray)
        {

            //TODO: documentation indicates uploadblob should overwrite, but it appears not to
            //workaround by deleteing for now
            await _blobContainerClient.DeleteBlobIfExistsAsync(blobPath).ConfigureAwait(false);
            await _blobContainerClient.UploadBlobAsync(blobPath, new MemoryStream(byteArray)).ConfigureAwait(false);
        }
    }
}
