using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;

namespace ST10257863.Functions
{
	public static class TableFunction
	{
		[Function("TableFunction")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			// Initialize table name and retrieve parameters from the query
			string tableName = "st10257863tablesservice";
			string partitionKey = req.Query["partitionKey"];
			string rowKey = req.Query["rowKey"];
			string firstName = req.Query["firstName"];
			string lastName = req.Query["lastName"];
			string email = req.Query["email"];
			string phoneNumber = req.Query["phoneNumber"];

			// Validate required parameters for null or empty values
			if (string.IsNullOrEmpty(tableName) ||
				string.IsNullOrEmpty(partitionKey) ||
				string.IsNullOrEmpty(rowKey) ||
				string.IsNullOrEmpty(firstName) ||
				string.IsNullOrEmpty(lastName) ||
				string.IsNullOrEmpty(email) ||
				string.IsNullOrEmpty(phoneNumber))
			{
				return new BadRequestObjectResult("Table name, partition key, row key, first name, last name, email, and phone number must be provided.");
			}

			// Connect to the Azure Table Service
			var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
			var serviceClient = new TableServiceClient(connectionString);
			var tableClient = serviceClient.GetTableClient(tableName);

			// Ensure the table exists before attempting to add data
			await tableClient.CreateIfNotExistsAsync();

			// Create the entity with the provided details
			var entity = new TableEntity(partitionKey, rowKey)
			{
				{ "FirstName", firstName },
				{ "LastName", lastName },
				{ "Email", email },
				{ "PhoneNumber", phoneNumber }
			};

			// Insert the new entity into the table
			await tableClient.AddEntityAsync(entity);

			// Return a success response indicating data was added
			return new OkObjectResult("Data added to table");
		}
	}
}
