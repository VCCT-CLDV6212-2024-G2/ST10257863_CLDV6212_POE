using Azure.Storage.Blobs;

namespace CLDV_POE_Web_Application.Services
{
	public class BlobService
	{
		private readonly BlobServiceClient _blobServiceClient;

		public BlobService(IConfiguration configuration)
		{
			_blobServiceClient = new BlobServiceClient(configuration["AzureStorage:ConnectionString"]);
		}

		public async Task UploadBlobAsync(string containerName, string blobName, Stream content)
		{
			var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			await containerClient.CreateIfNotExistsAsync();
			var blobClient = containerClient.GetBlobClient(blobName);
			await blobClient.UploadAsync(content, true);
		}
	}
}
