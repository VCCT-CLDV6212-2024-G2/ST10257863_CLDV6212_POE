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
			string shareName = "st10257863fileshare";
			string fileName = req.Query["fileName"];

			if (string.IsNullOrEmpty(shareName) || string.IsNullOrEmpty(fileName))
			{
				return new BadRequestObjectResult("Share name and file name must be provided.");
			}

			try
			{
				var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
				var shareServiceClient = new ShareServiceClient(connectionString);
				var shareClient = shareServiceClient.GetShareClient(shareName);
				await shareClient.CreateIfNotExistsAsync();
				var directoryClient = shareClient.GetRootDirectoryClient();
				var fileClient = directoryClient.GetFileClient(fileName);

				using var stream = req.Body;
				await fileClient.CreateAsync(stream.Length);
				await fileClient.UploadAsync(stream);

				return new OkObjectResult("File uploaded to Azure Files successfully.");
			}
			catch (Exception ex)
			{
				log.LogError(ex, "Error uploading file");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
