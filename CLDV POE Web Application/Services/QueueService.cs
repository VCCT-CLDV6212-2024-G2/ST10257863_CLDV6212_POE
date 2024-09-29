using Azure.Storage.Queues;

namespace CLDV_POE_Web_Application.Services
{
	public class QueueService
	{
		private readonly QueueServiceClient st10257863queueservice;

		public QueueService(IConfiguration configuration)
		{
			st10257863queueservice = new QueueServiceClient(configuration["AzureStorage:ConnectionString"]);
		}

		public async Task SendMessageAsync(string queueName, string message)
		{
			var queueClient = st10257863queueservice.GetQueueClient(queueName);
			await queueClient.CreateIfNotExistsAsync();
			await queueClient.SendMessageAsync(message);
		}
	}
}
