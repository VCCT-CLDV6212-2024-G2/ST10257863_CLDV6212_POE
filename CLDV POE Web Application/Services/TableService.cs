using Azure.Data.Tables;
using Azure.Storage.Queues;
using CLDV_POE_Web_Application.Models;
//Comment

namespace CLDV_POE_Web_Application.Services
{
	public class TableService
	{
		private readonly TableClient st10257863TableService;

		public TableService(IConfiguration configuration)
		{
			var connectionString = configuration["AzureStorage:ConnectionString"];
			var serviceClient = new TableServiceClient(connectionString);
			st10257863TableService = serviceClient.GetTableClient("st10257863tablesservice");
			st10257863TableService.CreateIfNotExists();
		}

		public async Task AddEntityAsync(CustomerProfile profile)
		{
			await st10257863TableService.AddEntityAsync(profile);
		}
	}
}
