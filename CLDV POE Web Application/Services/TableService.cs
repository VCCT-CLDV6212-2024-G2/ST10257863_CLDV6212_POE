using Azure.Data.Tables;
using CLDV_POE_Web_Application.Models;


namespace CLDV_POE_Web_Application.Services
{
	public class TableService
	{
		private readonly TableClient st10257864Tableservice;

		public TableService(IConfiguration configuration)
		{
			var connectionString = configuration["AzureStorage:ConnectionString"];
			var serviceClient = new TableServiceClient(connectionString);
			st10257864Tableservice = serviceClient.GetTableClient("CustomerProfiles");
			st10257864Tableservice.CreateIfNotExists();
		}

		public async Task AddEntityAsync(CustomerProfile profile)
		{
			await st10257864Tableservice.AddEntityAsync(profile);
		}
	}
}
