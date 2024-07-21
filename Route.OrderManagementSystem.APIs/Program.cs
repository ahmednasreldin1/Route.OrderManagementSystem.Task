
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Route.OrderManagementSystem.APIs.Extensions;
using Route.OrderManagementSystem.APIs.Middlewares;
using Route.OrderManagementSystem.Core.Data;
using Route.OrderManagementSystem.Core.Models.Identity;
using StackExchange.Redis;

namespace Route.OrderManagementSystem.APIs
{
	public class Program
	{
		public async static Task Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the container.

			webApplicationBuilder.Services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			});

			webApplicationBuilder.Services.AddSwaggerServices();

			webApplicationBuilder.Services.AddApplicationServices();

			webApplicationBuilder.Services.AddDbContext<OrderManagementDbContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"), options =>
				{
					options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(3), null);

				});
			});


			webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
			{
				var connection = webApplicationBuilder.Configuration.GetConnectionString("redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			webApplicationBuilder.Services.AddAuthServices(webApplicationBuilder.Configuration);


			#endregion

			var app = webApplicationBuilder.Build();

			#region Apply All Pending Migrations [Update-Database] and Data Seeding

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<OrderManagementDbContext>(); // Ask CLR for Creating Object from 'DbContext' Explicitly
			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			var logger = loggerFactory.CreateLogger<Program>();
			try
			{
				await _dbContext.Database.MigrateAsync(); // Update-Database 
				await OrderManagementDbContextSeed.SeedAsync(_dbContext); // Data Seeding

				var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
				await OrderManagementDbContextSeed.SeedUsersAsync(_userManager);
				await OrderManagementDbContextSeed.SeedRolesAsync(_roleManager);
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}


			#endregion

			#region Configure Kestrel Middlewares
			// Configure the HTTP request pipeline.

			app.UseMiddleware<ExceptionMiddleware>();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddlewares();

				//app.UseDeveloperExceptionPage();
			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.MapControllers();

			app.UseAuthentication();

			app.UseAuthorization();

			#endregion

			app.Run();
		}
	}
}
