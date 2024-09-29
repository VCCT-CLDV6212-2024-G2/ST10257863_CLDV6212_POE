using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table; // Ensure you have Microsoft.WindowsAzure.Storage package installed
using Microsoft.WindowsAzure.Storage;
using System;
using System.Threading.Tasks;

namespace ST10257863_AzureFunctions
{
	public static class StoreMessageInTable
	{
		// Define the table name and connection string
		private static string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
		private static CloudTable table;

		static StoreMessageInTable()
		{
			// Initialize the CloudTable object
			var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
			var tableClient = storageAccount.CreateCloudTableClient();
			table = tableClient.GetTableReference("QueueMessages"); // Table name can be customized
			table.CreateIfNotExistsAsync().Wait(); // Create the table if it doesn't exist
		}

		[Function("StoreMessageInTable")]
		public static async Task<IActionResult> RunAsync(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, ILogger log)
		{
			// Retrieve the queueName and message from query parameters
			string queueName = req.Query["queueName"];
			string message = req.Query["message"];

			if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(message))
			{
				return new BadRequestObjectResult("Queue name and message must be provided.");
			}

			// Create an entity to store in the Azure Table
			var entity = new DynamicTableEntity("QueueMessages", Guid.NewGuid().ToString())
			{
				Properties =
				{
					{ "QueueName", new EntityProperty(queueName) },
					{ "Message", new EntityProperty(message) },
					{ "Timestamp", new EntityProperty(DateTime.UtcNow) }
				}
			};

			// Insert or replace the entity
			var insertOperation = TableOperation.InsertOrReplace(entity);
			await table.ExecuteAsync(insertOperation);

			return new OkObjectResult($"Message stored in table: {entity.RowKey}");
		}
	}
}
