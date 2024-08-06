using Azure.Data.Tables;
using CLDV_POE_Web_Application.Models;


namespace CLDV_POE_Web_Application.Services
{
	public class TableService
	{
		private readonly TableClient _tableClient;

		public TableService(IConfiguration configuration)
		{
			var connectionString = configuration["AzureStorage:ConnectionString"];
			var serviceClient = new TableServiceClient(connectionString);
			_tableClient = serviceClient.GetTableClient("CustomerProfiles");
			_tableClient.CreateIfNotExists();
		}

		public async Task AddEntityAsync(CustomerProfile profile)
		{
			await _tableClient.AddEntityAsync(profile);
		}
	}
}
