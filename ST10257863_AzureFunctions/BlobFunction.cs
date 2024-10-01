using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ST10257863.Functions
{
	public static class BlobFunction
	{
		[Function("BlobFunction")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			// Initialize the container name and get the blob name from the query
			string containerName = "st10257863blobstorage";
			string blobName = req.Query["blobName"];

			// Validate input parameters
			if (string.IsNullOrEmpty(containerName) || string.IsNullOrEmpty(blobName))
			{
				return new BadRequestObjectResult("Container name and blob name must be provided.");
			}

			// Set up the Blob Service Client
			var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
			var blobServiceClient = new BlobServiceClient(connectionString);
			var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
			await containerClient.CreateIfNotExistsAsync();
			var blobClient = containerClient.GetBlobClient(blobName);

			// Upload the blob to the specified container
			using var stream = req.Body;
			await blobClient.UploadAsync(stream, true);

			// Return a success response
			return new OkObjectResult("Blob uploaded");
		}
	}
}
