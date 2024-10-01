using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
	.ConfigureFunctionsWebApplication() // This should work with the right packages
	.ConfigureServices(services =>
	{
		services.AddApplicationInsightsTelemetryWorkerService();
		services.ConfigureFunctionsApplicationInsights();
		// Register any services your application needs
	})
	.ConfigureWebJobs(b =>
	{
		// Register specific storage bindings
		b.AddHttp();
		b.AddAzureStorageBlobs(); // For Blob Storage functions
		b.AddAzureStorageQueues(); // For Queue Storage functions
								   // b.AddAzureStorageQueuesScaleForTrigger(); // Add this if scaling is needed for Queue Triggers
	})
	.Build();

host.Run();
//comment for commit.