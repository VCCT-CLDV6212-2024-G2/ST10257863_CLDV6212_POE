using Azure.Storage.Files.Shares;

namespace CLDV_POE_Web_Application.Services
{
	public class FileService
	{
		private readonly ShareServiceClient st10257863fileservice;

		public FileService(IConfiguration configuration)
		{
			st10257863fileservice = new ShareServiceClient(configuration["AzureStorage:ConnectionString"]);
		}

		public async Task UploadFileAsync(string shareName, string fileName, Stream content)
		{
			var shareClient = st10257863fileservice.GetShareClient(shareName);
			await shareClient.CreateIfNotExistsAsync();
			var directoryClient = shareClient.GetRootDirectoryClient();
			var fileClient = directoryClient.GetFileClient(fileName);
			await fileClient.CreateAsync(content.Length);
			await fileClient.UploadAsync(content);
		}
	}
}
