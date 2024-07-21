using Microsoft.AspNetCore.Identity;
using Route.OrderManagementSystem.Core.Models.Identity;
using Route.OrderManagementSystem.Core.Models.Order;
using Route.OrderManagementSystem.Core.Models.Product;
using System.Text.Json;

namespace Route.OrderManagementSystem.Core.Data
{
	public static class OrderManagementDbContextSeed
    {
        public async static Task SeedAsync(OrderManagementDbContext dbContext)
        {
			if (!dbContext.Products.Any())
			{
				var productsData = File.ReadAllText("../Route.OrderManagementSystem.Infrastructure/_Data/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productsData);

				if (products?.Count > 0)
				{

					foreach (var product in products)
					{
						dbContext.Set<Product>().Add(product);
					}
					await dbContext.SaveChangesAsync();
				}
			}
		}
		public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new ApplicationUser()
				{
					DisplayName = "Ahmed Nasr",
					Email = "ahmed.nasr@linkdev.com",
					UserName = "ahmed.nasr",
					PhoneNumber = "01122334455"
				};

				await userManager.CreateAsync(user, "P@ssw0rd");
			}
		}

		public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.Roles.Any())
			{
				await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
				await roleManager.CreateAsync(new IdentityRole() { Name = "Customer" });
			}
		}

	}
}
