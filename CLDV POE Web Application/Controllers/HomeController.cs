using CLDV_POE_Web_Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace CLDV_POE_Web_Application.Controllers
{
	public class HomeController : Controller
	{
		// Declare the HttpClientFactory to manage HTTP requests
		private readonly IHttpClientFactory _httpClientFactory;

		// Constructor to inject necessary services
		public HomeController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		// Default action to load the home page
		public IActionResult Index()
		{
			return View();
		}

		// Action to load the Privacy page
		public IActionResult Privacy()
		{
			return View();
		}

		// Private method to construct URLs
		private string BuildUrl(string functionName, string code, params (string key, string value)[] parameters)
		{
			var baseUrl = $"https://st10257863functionsapp.azurewebsites.net/api/{functionName}?code={code}";
			foreach (var param in parameters)
			{
				// URI escape each value and append as a query parameter
				baseUrl += $"&{param.key}={Uri.EscapeDataString(param.value)}";
			}
			return baseUrl;
		}

		// Action to handle image upload and send it to Blob Storage via HTTP POST request
		[HttpPost]
		public async Task<IActionResult> UploadImage(IFormFile file)
		{
			if (file != null)
			{
				using var stream = file.OpenReadStream();
				using var httpClient = _httpClientFactory.CreateClient();
				using var content = new StreamContent(stream);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

				// Use the file name as the blob name
				string blobName = file.FileName; // No need to escape here; it will be done in the URL

				// URL for the BlobFunction in Azure Functions
				string url = BuildUrl("BlobFunction", "tNd41756lZfQe_rT8x2B-5FrUJEqPIUt9Lgn9kUj7SmhAzFu5_iQGA%3D%3D",
					("blobName", blobName));

				// Send the POST request to upload the image
				var response = await httpClient.PostAsync(url, content);

				if (response.IsSuccessStatusCode)
				{
					// Handle successful upload, e.g., logging or feedback
				}
			}
			return RedirectToAction("Index");
		}

		// Action to process an order by sending it to a queue via HTTP POST request
		[HttpPost]
		public async Task<IActionResult> ProcessOrder(string orderId)
		{
			using var httpClient = _httpClientFactory.CreateClient();

			// URL for the QueueFunction in Azure Functions with the order ID
			string url = BuildUrl("QueueFunction", "SxRdtuPfdjzMFcoxx9nlbsaSYVDqjLo_7iWLAXuL9SxLAzFugtp3Aw%3D%3D",
				("message", $"OrderID: {orderId}")); // The message is also escaped

			// Send the POST request to process the order
			var response = await httpClient.PostAsync(url, null);

			if (response.IsSuccessStatusCode)
			{
				// Handle successful order processing
			}

			// Redirect to Index after processing the order
			return RedirectToAction("Index");
		}

		// Action to upload contract files via HTTP POST request
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

				// URL for the FileFunction in Azure Functions
				string url = BuildUrl("FileFunction", "QKG8YLt5hg5HAJqEFwXbtfAcsUbVw-G4EWa4TlNmcuRQAzFuAJFBRg%3D%3D",
					("fileName", fileName));

				// Send the POST request to upload the contract file
				var response = await httpClient.PostAsync(url, content);

				if (response.IsSuccessStatusCode)
				{
					// Handle successful upload
				}
			}

			// Redirect to Index after uploading the contract
			return RedirectToAction("Index");
		}

		// Action to add customer profile details to Azure Table Storage via HTTP POST request
		[HttpPost]
		public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
		{
			if (ModelState.IsValid) // Check if the submitted profile is valid
			{
				using var httpClient = _httpClientFactory.CreateClient();

				// Build the query parameters from the customer profile
				string partitionKey = profile.PartitionKey;
				string rowKey = profile.RowKey;
				string firstName = profile.FirstName;
				string lastName = profile.LastName;
				string email = profile.Email;
				string phoneNumber = profile.PhoneNumber;

				// URL for the TableFunction in Azure Functions
				string url = BuildUrl("TableFunction", "clrEXRJAKcqkLJWq3IVvOCL99RwMk7lrEPnZfZsYywqWAzFuS04tOg%3D%3D",
					("partitionKey", partitionKey),
					("rowKey", rowKey),
					("firstName", firstName),
					("lastName", lastName),
					("email", email),
					("phoneNumber", phoneNumber));

				// Send the POST request to add the profile
				var response = await httpClient.PostAsync(url, null);

				if (response.IsSuccessStatusCode)
				{
					// Handle success and redirect to Index
					return RedirectToAction("Index");
				}
				else
				{
					// Handle failure by adding an error message
					ModelState.AddModelError(string.Empty, "Failed to add the customer profile.");
				}
			}

			// If model validation fails, return the same view with the model
			return View(profile);
		}
	}
}
