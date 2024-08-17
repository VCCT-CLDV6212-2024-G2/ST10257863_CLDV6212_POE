using Azure.Storage.Blobs;

namespace CLDV_POE_Web_Application.Services
{
	public class BlobService
	{
		private readonly BlobServiceClient st10257863blobservice;

		public BlobService(IConfiguration configuration)
		{
			st10257863blobservice = new BlobServiceClient(configuration["AzureStorage:ConnectionString"]);
		}

		public async Task UploadBlobAsync(string containerName, string blobName, Stream content)
		{
			var containerClient = st10257863blobservice.GetBlobContainerClient(containerName);
			await containerClient.CreateIfNotExistsAsync();
			var blobClient = containerClient.GetBlobClient(blobName);
			await blobClient.UploadAsync(content, true);
		}
	}
}
