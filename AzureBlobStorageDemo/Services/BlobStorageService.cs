using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobStorageDemo.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobStorageClient;

        public BlobStorageService(string connection)
        {
            _blobStorageClient = new BlobServiceClient(connection);
        }

        public async Task UploadFileAsync(IFormFile file)
        {
            var containerClient = _blobStorageClient.GetBlobContainerClient("files");
            
            var blobClient = containerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                var response = await blobClient.UploadAsync(stream, new BlobUploadOptions()
                {
                    HttpHeaders = new BlobHttpHeaders() { ContentType = file.ContentType }
                });

            }
        }

        public async Task<List<string>> ListFiles()
        {
            var containerClient = _blobStorageClient.GetBlobContainerClient("files");
            List<string> result = new List<string>();

            await foreach (var blob in containerClient.GetBlobsAsync())
            {
                result.Add(blob.Name);
            }
            return result;
        }
    }
}
