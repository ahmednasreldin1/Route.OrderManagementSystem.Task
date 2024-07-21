using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Route.OrderManagementSystem.Core.Models.Customer;
using Route.OrderManagementSystem.Core.Models.Identity;
using Route.OrderManagementSystem.Core.Models.Invoice;
using Route.OrderManagementSystem.Core.Models.Order;
using Route.OrderManagementSystem.Core.Models.Product;
using System.Reflection;

namespace Route.OrderManagementSystem.Core.Data
{
    public class OrderManagementDbContext : IdentityDbContext<ApplicationUser>
	{
		public OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}


		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Invoice> Invoices { get; set; }
	}
}
