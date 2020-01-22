using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NETToolBox.BlobStorage.Abstractions;
using System.IO;
using System.Text;
using System.Threading;
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

        public async Task<Stream> DownloadFileAsStreamAsync(string blobPath, CancellationToken cancellationToken = default)
        {

            var blob = _blobContainerClient.GetBlobClient(blobPath);
            using BlobDownloadInfo download = await blob.DownloadAsync(cancellationToken).ConfigureAwait(false);
            return download.Content;
        }

        public async Task<string> DownloadFileAsTextAsync(string blobPath, CancellationToken cancellationToken = default)
        {
            string retval;
            var download = await DownloadFileAsStreamAsync(blobPath,cancellationToken).ConfigureAwait(false);
            using (var sr = new StreamReader(download))
            {
                retval = await sr.ReadToEndAsync().ConfigureAwait(false);
            }
            return retval;
        }
      
        public async Task StoreBlobAsBytesAsync(string blobPath, byte[] byteArray, CancellationToken cancellationToken = default)
        {
            Stream ms = new MemoryStream(byteArray);
            await StoreBlobAsStreamAsync(blobPath, ms, cancellationToken).ConfigureAwait(false);
        }

      

        public async Task StoreBlobAsStreamAsync(string blobPath, Stream stream, CancellationToken cancellationToken = default)
        {
            //TODO: documentation indicates uploadblob should overwrite, but it appears not to
            //workaround by deleteing for now
            await _blobContainerClient.DeleteBlobIfExistsAsync(blobName:blobPath,cancellationToken:cancellationToken).ConfigureAwait(false);
            await _blobContainerClient.UploadBlobAsync(blobPath, stream, cancellationToken).ConfigureAwait(false);
        }
     
        public async Task StoreBlobAsTextAsync(string blobPath, string blobContents, CancellationToken cancellationToken = default)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(blobContents);
            await StoreBlobAsBytesAsync(blobPath, byteArray, cancellationToken).ConfigureAwait(false) ;
        }
    }
}
