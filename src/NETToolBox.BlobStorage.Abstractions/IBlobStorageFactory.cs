namespace NETToolBox.BlobStorage.Abstractions
{
    public interface IBlobStorageFactory
    {
        IBlobStorage GetBlobStorage(string connectionString, string containerName);
    }
}
