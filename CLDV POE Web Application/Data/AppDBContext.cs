using Microsoft.EntityFrameworkCore;
using CLDV_POE_Web_Application.Models.Tables;

namespace CLDV_POE_Web_Application.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<CustomerProfile> Customers
		{
			get; set;
		}
		public DbSet<Product> Products
		{
			get; set;
		}
		public DbSet<Order> Orders
		{
			get; set;
		}
	}
}
