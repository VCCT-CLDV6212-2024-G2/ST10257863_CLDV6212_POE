using CLDV_POE_Web_Application.Models;
using CLDV_POE_Web_Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CLDV_POE_Web_Application.Controllers
{
	public class HomeController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory; // Declare the HttpClient factory
		private readonly BlobService _blobService;
		private readonly TableService _tableService;
		private readonly QueueService _queueService;
		private readonly FileService _fileService;

		public HomeController(
			IHttpClientFactory httpClientFactory, // Inject IHttpClientFactory
			BlobService blobService,
			TableService tableService,
			QueueService queueService,
			FileService fileService)
		{
			_httpClientFactory = httpClientFactory;
			_blobService = blobService;
			_tableService = tableService;
			_queueService = queueService;
			_fileService = fileService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UploadImage(IFormFile file)
		{
			if (file != null)
			{
				using var stream = file.OpenReadStream();
				using var httpClient = _httpClientFactory.CreateClient(); // Use injected HttpClientFactory
				using var content = new StreamContent(stream);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

				// Use the file name as the blob name
				string blobName = file.FileName;

				// Construct the URL with the necessary query parameter for the blob name
				string url = $"https://st10257863functionsapp.azurewebsites.net/api/BlobFunction?code=tNd41756lZfQe_rT8x2B-5FrUJEqPIUt9Lgn9kUj7SmhAzFu5_iQGA%3D%3D&blobName={blobName}";

				// Send the POST request
				var response = await httpClient.PostAsync(url, content);

				if (response.IsSuccessStatusCode)
				{
					// Handle success (optional logging or user feedback)
				}
			}
			return RedirectToAction("Index");
		}


		[HttpPost]
		public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
		{
			if (ModelState.IsValid)
			{
				using var httpClient = _httpClientFactory.CreateClient();

				// Use the properties from the profile to build the data for Azure Table Storage
				string partitionKey = profile.PartitionKey; // Example: could be a derived value
				string rowKey = profile.RowKey;             // Example: could be a derived value
				string data = JsonConvert.SerializeObject(new
				{
					profile.FirstName,
					profile.LastName,
					profile.Email,
					profile.PhoneNumber
				});

				// Construct the URL with the necessary query parameters
				string url = $"https://st10257863functionsapp.azurewebsites.net/api/TableFunction?code=clrEXRJAKcqkLJWq3IVvOCL99RwMk7lrEPnZfZsYywqWAzFuS04tOg%3D%3D&partitionKey={partitionKey}&rowKey={rowKey}&data={data}";

				// Send the POST request
				var response = await httpClient.PostAsync(url, null);

				if (response.IsSuccessStatusCode)
				{
					// Handle success (optional logging or user feedback)
				}
				else
				{
					// Optionally handle the error response
				}
			}
			return RedirectToAction("Index");
		}


		[HttpPost]
		public async Task<IActionResult> ProcessOrder(string orderId)
		{
			using var httpClient = _httpClientFactory.CreateClient();

			// Prepare the URL with the query parameter for the message
			string url = $"https://st10257863functionsapp.azurewebsites.net/api/QueueFunction?code=SxRdtuPfdjzMFcoxx9nlbsaSYVDqjLo_7iWLAXuL9SxLAzFugtp3Aw%3D%3D&message={orderId}";

			// Send the POST request
			var response = await httpClient.PostAsync(url, null);

			if (response.IsSuccessStatusCode)
			{
				// Handle success (optional logging or user feedback)
			}
			else
			{
				// Optionally handle the error response
			}

			return RedirectToAction("Index");
		}


		[HttpPost]
		public async Task<IActionResult> UploadContract(IFormFile file)
		{
			if (file != null)
			{
				using var stream = file.OpenReadStream();
				using var httpClient = _httpClientFactory.CreateClient();
				using var content = new StreamContent(stream);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

				// Use the file name as the file name parameter
				string fileName = file.FileName;

				// Construct the URL with the necessary query parameter for the file name
				string url = $"https://st10257863functionapp.azurewebsites.net/api/FileFunction?code=s3vUM0zqORkiDMBoNJmX3GZnxckfv8rRSomCw9_5fGujAzFuA-vecw%3D%3D&fileName={fileName}";

				// Send the POST request
				var response = await httpClient.PostAsync(url, content);

				if (response.IsSuccessStatusCode)
				{
					// Handle success (optional logging or user feedback)
				}
			}
			return RedirectToAction("Index");
		}

	}
}
