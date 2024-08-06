using Azure;
using Azure.Data.Tables; // Ensure you are using the correct namespace

namespace CLDV_POE_Web_Application.Models
{
	public class CustomerProfile : ITableEntity
	{
		public string PartitionKey
		{
			get; set;
		}
		public string RowKey
		{
			get; set;
		}
		public DateTimeOffset? Timestamp
		{
			get; set;
		}
		public ETag ETag
		{
			get; set;
		}

		// Custom properties
		public string FirstName
		{
			get; set;
		}
		public string LastName
		{
			get; set;
		}
		public string Email
		{
			get; set;
		}
		public string PhoneNumber
		{
			get; set;
		}

		public CustomerProfile()
		{
			PartitionKey = "CustomerProfile";
			RowKey = Guid.NewGuid().ToString();
		}
	}
}
