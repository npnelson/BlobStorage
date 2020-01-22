using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NETToolBox.BlobStorage.Abstractions
{
    public interface IBlobStorage
    {
        Task<string> DownloadFileAsTextAsync(string blobPath, CancellationToken cancellationToken = default);
        Task StoreBlobAsBytesAsync(string blobPath, byte[] byteArray, CancellationToken cancellationToken=default);
        Task StoreBlobAsTextAsync(string blobPath, string blobContents, CancellationToken cancellationToken= default);
        Task StoreBlobAsStreamAsync(string blobPath, Stream stream,CancellationToken cancellationToken= default);
        Task<Stream> DownloadFileAsStreamAsync(string blobPath, CancellationToken cancellationToken = default);
    }
}
