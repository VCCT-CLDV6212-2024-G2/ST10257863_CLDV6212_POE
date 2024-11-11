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
		public DateTime OrderDate
		{
			get; set;
		}
		[Required]
		public string Status
		{
			get; set;
		} // e.g., Pending, Completed
	}
}
