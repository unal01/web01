using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoreBuilder.Services
{
    public class AzureBlobFileStorageService : IFileStorageService
    {
        private readonly BlobContainerClient _container;

        public AzureBlobFileStorageService(string connectionString, string containerName)
        {
            _container = new BlobContainerClient(connectionString, containerName);
            _container.CreateIfNotExists();
        }

        public async Task<string> UploadAsync(Stream stream, string contentType, string fileName)
        {
            var blobClient = _container.GetBlobClient(fileName);
            await blobClient.UploadAsync(stream, overwrite: true);
            return blobClient.Uri.ToString();
        }

        public async Task<Stream?> DownloadAsync(string filePath)
        {
            var blobClient = new BlobClient(new Uri(filePath));
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public async Task DeleteAsync(string filePath)
        {
            var blobClient = new BlobClient(new Uri(filePath));
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
