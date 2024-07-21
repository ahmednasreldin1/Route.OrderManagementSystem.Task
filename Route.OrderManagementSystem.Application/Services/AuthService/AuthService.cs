using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Route.OrderManagementSystem.Core.Contracts.Services;
using Route.OrderManagementSystem.Core.Models.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Route.OrderManagementSystem.Application.Services.AuthService
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _configuration;

		public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
		}


		public async Task<string> CreateTokenAsync(ApplicationUser user)
		{
			// Private Claims
			var authClaims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.DisplayName!),
				new Claim(ClaimTypes.Email, user.Email!),
			};

			var userRoles = await _userManager.GetRolesAsync(user);

			foreach (var role in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, role));
			}

			var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"] ?? string.Empty));

			var token = new JwtSecurityToken(

				audience: _configuration["JWT:ValidAudience"],
				issuer: _configuration["JWT:ValidIssuer"],
				expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"] ?? "0")),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)

				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

	}

}
