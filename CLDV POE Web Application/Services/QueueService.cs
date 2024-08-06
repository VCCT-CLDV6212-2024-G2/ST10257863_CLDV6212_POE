﻿using Azure.Storage.Queues;

namespace CLDV_POE_Web_Application.Services
{
	public class QueueService
	{
		private readonly QueueServiceClient _queueServiceClient;

		public QueueService(IConfiguration configuration)
		{
			_queueServiceClient = new QueueServiceClient(configuration["AzureStorage:ConnectionString"]);
		}

		public async Task SendMessageAsync(string queueName, string message)
		{
			var queueClient = _queueServiceClient.GetQueueClient(queueName);
			await queueClient.CreateIfNotExistsAsync();
			await queueClient.SendMessageAsync(message);
		}
	}
}