using CLDV_POE_Web_Application.Data;
using CLDV_POE_Web_Application.Models.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration; // Added for IConfiguration
using System;
using System.Threading.Tasks;

namespace CLDV_POE_Web_Application.Controllers
{
	public class HomeController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly AppDbContext _context;
		private readonly ILogger<HomeController> _logger;
		private readonly IConfiguration _configuration; // Added for IConfiguration

		// Constructor to inject necessary services
		public HomeController(IHttpClientFactory httpClientFactory, AppDbContext context, ILogger<HomeController> logger, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_context = context;
			_logger = logger;
			_configuration = configuration; // Initialize the IConfiguration
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

		// Action to handle image upload and send it to Blob Storage via HTTP POST request
		[HttpPost]
		public async Task<IActionResult> UploadImage(IFormFile file)
		{
			if (file != null && file.Length > 0)
			{
				try
				{
					using var stream = file.OpenReadStream();
					using var httpClient = _httpClientFactory.CreateClient();
					using var content = new StreamContent(stream);
					content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

					// Use the file name as the blob name
					string blobName = file.FileName;

					// Retrieve the full URL for the BlobFunction from appsettings.json
					string url = _configuration["AzureFunctions:BlobFunctionUrl"];

					// Send the POST request to upload the image
					var response = await httpClient.PostAsync(url, content);

					if (!response.IsSuccessStatusCode)
					{
						_logger.LogError($"Error uploading image: {response.ReasonPhrase}");
					}
				}
				catch (Exception ex)
				{
					_logger.LogError($"Exception occurred during image upload: {ex.Message}");
				}
			}
			else
			{
				_logger.LogWarning("No image file provided for upload.");
			}

			return RedirectToAction("Index");
		}

		// Action to upload contract files via HTTP POST request
		[HttpPost]
		public async Task<IActionResult> UploadContract(IFormFile file)
		{
			if (file != null && file.Length > 0)
			{
				try
				{
					using var stream = file.OpenReadStream();
					using var httpClient = _httpClientFactory.CreateClient();
					using var content = new StreamContent(stream);
					content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

					// Retrieve the full URL for the FileFunction from appsettings.json
					string url = _configuration["AzureFunctions:FileFunctionUrl"];

					// Send the POST request to upload the contract file
					var response = await httpClient.PostAsync(url, content);

					if (!response.IsSuccessStatusCode)
					{
						_logger.LogError($"Error uploading contract: {response.ReasonPhrase}");
					}
				}
				catch (Exception ex)
				{
					_logger.LogError($"Exception occurred during contract upload: {ex.Message}");
				}
			}
			else
			{
				_logger.LogWarning("No contract file provided for upload.");
			}

			return RedirectToAction("Index");
		}

		// Action to add a customer profile to the database
		[HttpPost]
		public async Task<IActionResult> AddCustomerProfile(CustomerProfile customer)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_context.Customers.Add(customer);
					await _context.SaveChangesAsync();
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					_logger.LogError($"Exception occurred while saving customer profile: {ex.Message}");
				}
			}

			return View(customer);
		}

		// Action to add a product to the database
		[HttpPost]
		public async Task<IActionResult> AddProduct(Product product)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_context.Products.Add(product);
					await _context.SaveChangesAsync();
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					_logger.LogError($"Exception occurred while saving product: {ex.Message}");
				}
			}

			return View(product);
		}

		// Action to add an order to the database
		[HttpPost]
		public async Task<IActionResult> ProcessOrder(string orderId)
		{
			if (string.IsNullOrWhiteSpace(orderId))
			{
				_logger.LogWarning("Order ID cannot be empty.");
				return RedirectToAction("Index");
			}

			try
			{
				// Create a new Order and set the status to Pending
				var newOrder = new Order
				{
					OrderDate = DateTime.Now,
					Status = "Pending"
				};

				// Add the new order to the Orders table
				_context.Orders.Add(newOrder);
				await _context.SaveChangesAsync();

				_logger.LogInformation($"New order with ID {newOrder.OrderId} created with Pending status.");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Exception occurred while processing order: {ex.Message}");
			}

			return RedirectToAction("Index");
		}
	}
}
