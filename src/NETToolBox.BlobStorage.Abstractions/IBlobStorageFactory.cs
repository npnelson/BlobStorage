namespace NETToolBox.BlobStorage.Abstractions
{
    public interface IBlobStorageFactory
    {
        /// <summary>
        /// Gets a container from the accountName. The Azure implementation (which is the only implementation we are worried about right now uses a DefaultAzureCredential https://docs.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential?view=azure-dotnet which encourages the use of managed identity
        /// It will also create the container if it doesnt exist
        /// </summary>
        /// <param name="accountName">Storage account name (note this is not a connection string! just the account name</param>
        /// <param name="containerName">ContainerName</param>
        /// <returns></returns>
        IBlobStorage GetBlobStorage(string accountName, string containerName);
    }
}
