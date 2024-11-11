using System.ComponentModel.DataAnnotations;

namespace CLDV_POE_Web_Application.Models.Tables
{
	public class Product
	{
		[Key]
		public int ProductId
		{
			get; set;
		}
		[Required]
		public string Name
		{
			get; set;
		}
		[Required]
		public decimal Price
		{
			get; set;
		}

	}
}
