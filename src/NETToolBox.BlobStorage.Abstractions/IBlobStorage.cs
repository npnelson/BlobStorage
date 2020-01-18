using System.IO;
using System.Threading.Tasks;

namespace NETToolBox.BlobStorage.Abstractions
{
    public interface IBlobStorage
    {
        Task<string> DownloadFileAsTextAsync(string blobPath);
        Task StoreBlobAsBytesAsync(string blobPath, byte[] byteArray);
        Task<Stream> DownloadFileAsStreamAsync(string blobPath);
    }
}
