using Azure.Data.Tables;
using Azure.Storage.Queues;
using CLDV_POE_Web_Application.Models;
//Comment

namespace CLDV_POE_Web_Application.Services
{
	public class TableService
	{
		private readonly TableClient st10257864TableService;

		public TableService(IConfiguration configuration)
		{
			var connectionString = configuration["AzureStorage:ConnectionString"];
			var serviceClient = new TableServiceClient(connectionString);
			st10257864TableService = serviceClient.GetTableClient("st10257864tablesservice");
			st10257864TableService.CreateIfNotExists();
		}

		public async Task AddEntityAsync(CustomerProfile profile)
		{
			await st10257864TableService.AddEntityAsync(profile);
		}
	}
}
