using System.ComponentModel.DataAnnotations;

namespace CLDV_POE_Web_Application.Models.Tables
{
	public class CustomerProfile
	{
		[Key]
		public int CustomerProfileId
		{
			get; set;
		} // Primary Key
		[Required]
		public string FirstName
		{
			get; set;
		}
		[Required]
		public string LastName
		{
			get; set;
		}
		[Required]
		public string Email
		{
			get; set;
		}
		[Required]
		public string PhoneNumber
		{
			get; set;
		}
	}
}
