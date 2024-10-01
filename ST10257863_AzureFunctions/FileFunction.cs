using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ST10257863.Functions
{
	public static class FileFunction
	{
		[Function("FileFunction")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			// Initialize share name and retrieve the file name from the query
			string shareName = "st10257863fileshare";
			string fileName = req.Query["fileName"];

			// Validate that both the share name and file name are provided
			if (string.IsNullOrEmpty(shareName) || string.IsNullOrEmpty(fileName))
			{
				return new BadRequestObjectResult("Share name and file name must be provided.");
			}

			try
			{
				// Connect to the Azure File Share Service
				var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
				var shareServiceClient = new ShareServiceClient(connectionString);
				var shareClient = shareServiceClient.GetShareClient(shareName);
				await shareClient.CreateIfNotExistsAsync();
				var directoryClient = shareClient.GetRootDirectoryClient();
				var fileClient = directoryClient.GetFileClient(fileName);

				// Read the request body into a stream for upload
				using var stream = new MemoryStream();
				await req.Body.CopyToAsync(stream);
				stream.Position = 0; // Reset stream position for upload
				await fileClient.CreateAsync(stream.Length); // Create the file in Azure
				stream.Position = 0; // Reset stream position for upload
				await fileClient.UploadAsync(stream); // Upload the file stream

				// Return a success response
				return new OkObjectResult("File uploaded to Azure Files successfully.");
			}
			catch (Exception ex)
			{
				// Log any errors that occur during the upload process
				log.LogError(ex, "Error uploading file");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError); // Return a server error response
			}
		}
	}
}
