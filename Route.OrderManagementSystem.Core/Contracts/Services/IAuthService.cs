using Microsoft.AspNetCore.Identity;
using Route.OrderManagementSystem.Core.Models.Identity;

namespace Route.OrderManagementSystem.Core.Contracts.Services
{
	public interface IAuthService
	{
		Task<string> CreateTokenAsync(ApplicationUser user);
	}
}
