
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Route.OrderManagementSystem.APIs.Errors;
using Route.OrderManagementSystem.APIs.Helpers;
using Route.OrderManagementSystem.Application.Services.AuthService;
using Route.OrderManagementSystem.Application.Services.CacheService;
using Route.OrderManagementSystem.Application.Services.OrderService;
using Route.OrderManagementSystem.Application.Services.ProductService;
using Route.OrderManagementSystem.Core.Contracts.Services;
using Route.OrderManagementSystem.Core.Contracts.UnitOfWork;
using Route.OrderManagementSystem.Core.Data;
using Route.OrderManagementSystem.Core.Models.Identity;
using Route.OrderManagementSystem.Infrastructure.UnitOfWork;
using System.Text;

namespace Route.OrderManagementSystem.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{

			services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));


			services.AddScoped(typeof(IProductService), typeof(ProductService));

			services.AddScoped(typeof(IOrderService), typeof(OrderService));

			services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

			services.AddAutoMapper(typeof(MappingProfiles));

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{

					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
														 .SelectMany(P => P.Value.Errors)
														 .Select(E => E.ErrorMessage)
														 .ToList();

					var validationErrorResponse = new ApiValidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(validationErrorResponse);
				};
			});

			return services;

		}

		public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				//options.Password.RequiredUniqueChars = 2;
				//options.Password.RequireDigit = true;
				//options.Password.RequireLowercase = true;
				//options.Password.RequireUppercase = true;
			})
				.AddEntityFrameworkStores<OrderManagementDbContext>();
			services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/ options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = configuration["JWT:ValidIssuer"],
						ValidateAudience = true,
						ValidAudience = configuration["JWT:ValidAudience"],
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"] ?? string.Empty)),
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero,
					};
				});

			services.AddScoped(typeof(IAuthService), typeof(AuthService));

			return services;
		}
	}
}
