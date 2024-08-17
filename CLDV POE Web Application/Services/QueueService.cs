using Azure.Storage.Queues;

namespace CLDV_POE_Web_Application.Services
{
	public class QueueService
	{
		private readonly QueueServiceClient st10257864queueservice;

		public QueueService(IConfiguration configuration)
		{
			st10257864queueservice = new QueueServiceClient(configuration["AzureStorage:ConnectionString"]);
		}

		public async Task SendMessageAsync(string queueName, string message)
		{
			var queueClient = st10257864queueservice.GetQueueClient(queueName);
			await queueClient.CreateIfNotExistsAsync();
			await queueClient.SendMessageAsync(message);
		}
	}
}
