using CLDV_POE_Web_Application.Models;
using CLDV_POE_Web_Application.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace CLDV_POE_Web_Application.Controllers
{
	public class HomeController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory; // Declare the HttpClient factory
		private readonly BlobService _blobService;
		private readonly TableService _tableService;
		private readonly QueueService _queueService;
		private readonly FileService _fileService;

		//public HomeController(BlobService blobService, TableService tableService, QueueService queueService, FileService fileService)
		//{
		//	_blobService = blobService;
		//	_tableService = tableService;
		//	_queueService = queueService;
		//	_fileService = fileService;
		//}
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

		//[HttpPost]
		//public async Task<IActionResult> UploadImage(IFormFile file)
		//{
		//	if (file != null)
		//	{
		//		using var stream = file.OpenReadStream();
		//		await _blobService.UploadBlobAsync("st10257863blobservice", file.FileName, stream);
		//	}
		//	return RedirectToAction("Index");
		//}

		//[HttpPost]
		//public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		await _tableService.AddEntityAsync(profile);
		//	}
		//	return RedirectToAction("Index");
		//}

		//[HttpPost]
		//public async Task<IActionResult> ProcessOrder(string orderId)
		//{
		//	await _queueService.SendMessageAsync("st10257863queueservice", $"Processing order {orderId}");
		//	return RedirectToAction("Index");
		//}

		//[HttpPost]
		//public async Task<IActionResult> UploadContract(IFormFile file)
		//{
		//	if (file != null)
		//	{
		//		using var stream = file.OpenReadStream();
		//		await _fileService.UploadFileAsync("st10257863fileservice", file.FileName, stream);
		//	}
		//	return RedirectToAction("Index");
		//}
		[HttpPost]
		public async Task<IActionResult> UploadImage(IFormFile file)
		{
			if (file != null)
			{
				using var stream = file.OpenReadStream();

				using var httpClient = new HttpClient();
				var content = new StreamContent(stream);
				var response = await httpClient.PostAsync("https://st10257863functionapp.azurewebsites.net/api/BlobFunction?code=jk2fhFppFhcTF276nz2JCjWZgaKG62gyWGyp5Re2_C2IAzFuDS9CoQ%3D%3D", content);

				if (response.IsSuccessStatusCode)
				{
					// Handle success
				}
			}
			return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
		{
			if (ModelState.IsValid)
			{
				using var httpClient = new HttpClient();
				var json = JsonConvert.SerializeObject(profile);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				var response = await httpClient.PostAsync("https://st10257863functionapp.azurewebsites.net/api/TableFunction?code=nLaAhQqepu-wDvatVW1dtJqEsuaprzoMYQd9ifLBpxmwAzFud4-PiQ%3D%3D", content);

				if (response.IsSuccessStatusCode)
				{
					// Handle success
				}
			}
			return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> ProcessOrder(string orderId)
		{
			using var httpClient = new HttpClient();
			var content = new StringContent($"{{\"orderId\": \"{orderId}\"}}", Encoding.UTF8, "application/json");

			var response = await httpClient.PostAsync("https://st10257863functionapp.azurewebsites.net/api/QueueFunction?code=DEOlDPtd_twO1jmbnzfLV1-F6jm032gdXvRiV1o7ThSIAzFuR3MLmg%3D%3D", content);

			if (response.IsSuccessStatusCode)
			{
				// Handle success
			}
			return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> UploadContract(IFormFile file)
		{
			if (file != null)
			{
				using var stream = file.OpenReadStream();

				using var httpClient = new HttpClient();
				var content = new StreamContent(stream);
				var response = await httpClient.PostAsync("https://st10257863functionapp.azurewebsites.net/api/FileFunction?code=s3vUM0zqORkiDMBoNJmX3GZnxckfv8rRSomCw9_5fGujAzFuA-vecw%3D%3D", content);

				if (response.IsSuccessStatusCode)
				{
					// Handle success
				}
			}
			return RedirectToAction("Index");
		}
	}
}
