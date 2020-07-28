using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NETToolBox.BlobStorage.Abstractions
{
    public interface IBlobStorage
    {
        Task<string> GetContentTypeAsync(string blobPath, CancellationToken cancellationToken = default);
        Task<string> DownloadFileAsTextAsync(string blobPath, CancellationToken cancellationToken = default);
        Task StoreBlobAsBytesAsync(string blobPath, byte[] byteArray, CancellationToken cancellationToken = default);
        Task StoreBlobAsTextAsync(string blobPath, string blobContents, CancellationToken cancellationToken = default);
        Task StoreBlobAsStreamAsync(string blobPath, Stream stream, CancellationToken cancellationToken = default);
        Task StoreBlobAsStreamAsync(string blobPath, Stream stream, string contentType, CancellationToken cancellationToken = default);
        Task<Stream> DownloadFileAsStreamAsync(string blobPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// If the container can be accessed, it returns true, if it can't, it will return false or throw an exception
        /// </summary>
        /// <returns></returns>
        Task<bool> IsHealthyAsync();
    }
}
