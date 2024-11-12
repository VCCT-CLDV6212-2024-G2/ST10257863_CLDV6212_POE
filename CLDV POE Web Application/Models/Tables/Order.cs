using System.ComponentModel.DataAnnotations;

namespace CLDV_POE_Web_Application.Models.Tables
{
	public class Order
	{
		[Key]
		public int OrderId
		{
			get; set;
		} // Primary Key

		[Required]
		public string OrderNumber
		{
			get; set;
		}

		[Required]
		public DateTime OrderDate
		{
			get; set;
		}

		[Required]
		public string Status
		{
			get; set;
		} = "pending"; // e.g., Pending, Completed. Default state is pending
	}
}
