using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Route.OrderManagementSystem.APIs.DTOs;
using Route.OrderManagementSystem.APIs.Errors;
using Route.OrderManagementSystem.Core.Contracts.Services;
using Route.OrderManagementSystem.Core.Models.Customer;
using Route.OrderManagementSystem.Core.Models.Order;
using System.Security.Claims;

namespace Route.OrderManagementSystem.APIs.Controllers
{
	public class CustomerController : BaseApiController
	{
		private readonly ICustomerService _customerService;

		public CustomerController(ICustomerService customerRepository)
		{
			_customerService = customerRepository;
		}


		[HttpPost]
		public async Task<ActionResult<Customer>> CreateCustomer(CustomerDto customerDto)
		{
			var customer = new Customer()
			{
				User = new Core.Models.Identity.ApplicationUser()
				{
					DisplayName = customerDto.DisplayName,
					Email = customerDto.Email,
					PhoneNumber = customerDto.PhoneNumber,
				}
			};

			var createdCustomer = await _customerService.CreateCustomerAsync(customer, customerDto.Password);

			customerDto.Id = createdCustomer.Id;

			return CreatedAtAction(nameof(GetCustomerOrders), new { customerId = customerDto.Id }, createdCustomer);
		}

		[Authorize]
		[ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("orders")]
		public async Task<ActionResult<IReadOnlyList<Order>>> GetCustomerOrders()
		{
			var customerEmail = User.FindFirstValue(ClaimTypes.Email)!;

			var orders = await _customerService.GetCustomerOrdersAsync(customerEmail);
			if (orders == null || !orders.Any())
			{
				return NotFound(new ApiResponse(404, "No orders found for this customer."));
			}
			return Ok(orders);
		}
	}
}
