using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.OrderManagementSystem.APIs.DTOs;
using Route.OrderManagementSystem.APIs.Errors;
using Route.OrderManagementSystem.Core.Contracts.Services;
using Route.OrderManagementSystem.Core.Models.Customer;
using Route.OrderManagementSystem.Core.Models.Identity;

namespace Route.OrderManagementSystem.APIs.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IAuthService _authService;

		public AccountController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
			IAuthService authService)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_authService = authService;
		}

		[HttpPost("login")] // POST : /api/Account/login
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is null) return Unauthorized(new ApiResponse(401, "Invalid Login"));

			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

			if(!result.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid Login"));

			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName!,
				Email = user.Email!,
				Token = await _authService.CreateTokenAsync(user)
			});
		}


		[HttpPost("register")] // POST : /api/Account/register
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{

			if (EmailExists(model.Email).Result.Value)
				return BadRequest(new ApiValidationErrorResponse() { Errors = new[] { "This Email is already in use." } });

			var user = new ApplicationUser()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				UserName = model.Email.Split("@")[0],
				PhoneNumber = model.Phone
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			await _userManager.AddToRoleAsync(user, "Admin");


			if (!result.Succeeded) return BadRequest(new ApiValidationErrorResponse() { Errors = result.Errors.Select(E => E.Description) });

			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _authService.CreateTokenAsync(user)
			});
		}

		[HttpGet("emailexists")] // GET : /api/Account/emailexists?email=
		public async Task<ActionResult<bool>> EmailExists(string email)
		{
			return await _userManager.FindByEmailAsync(email) is not null;
		}
	}
}
