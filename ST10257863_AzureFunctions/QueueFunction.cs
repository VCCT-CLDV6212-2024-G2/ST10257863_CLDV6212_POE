using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ST10257863.Functions
{
	public static class QueueFunction
	{
		[Function("QueueFunction")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			// Initialize queue name and retrieve the message from the query
			string queueName = "st10257863queueservice";
			string message = req.Query["message"];

			// Validate that both the queue name and message are provided
			if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(message))
			{
				return new BadRequestObjectResult("Queue name and message must be provided.");
			}

			// Connect to the Azure Queue Service
			var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
			var queueServiceClient = new QueueServiceClient(connectionString);
			var queueClient = queueServiceClient.GetQueueClient(queueName);

			// Ensure the queue exists before sending a message
			await queueClient.CreateIfNotExistsAsync();

			// Send the message to the specified queue
			await queueClient.SendMessageAsync(message);

			// Return a success response indicating the message was added
			return new OkObjectResult("Message added to queue");
		}
	}
}
