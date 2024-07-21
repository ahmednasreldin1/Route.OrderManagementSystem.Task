using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Route.OrderManagementSystem.APIs.Dtos;
using Route.OrderManagementSystem.APIs.Errors;
using Route.OrderManagementSystem.Application.Services.OrderService;
using Route.OrderManagementSystem.Core.Models.Order;
using System.Security.Claims;

namespace Route.OrderManagementSystem.APIs.Controllers
{

	public class OrdersController : BaseApiController
	{
		private readonly OrderService _orderService;

		public OrdersController(OrderService orderService)
		{
			_orderService = orderService;
		}

		[HttpPost]
		[Authorize(Roles = "Customer")]
		public async Task<IActionResult> CreateOrder(OrderDto orderDto)
		{
			var customerEmail = User.FindFirstValue(ClaimTypes.Email)!;

			var order = new Order()
			{
				CustomerEmail = customerEmail,
				PaymentMethod = orderDto.PaymentMethod,
				Items = orderDto.Items,
			};

			var createdOrder = await _orderService.CreateOrderAsync(order);
			return CreatedAtAction(nameof(GetOrder), new { orderId = createdOrder?.Id }, createdOrder);
		}

		[Authorize]
		[HttpGet("{orderId}")]
		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Order>> GetOrder(int orderId)
		{
			var customerEmail = User.FindFirstValue(ClaimTypes.Email)!;

			var order = await _orderService.GetOrderByIdForUserAsync(customerEmail, orderId);

			if (order == null)
			{
				return NotFound(new ApiResponse(404));
			}

			return Ok(order);
		}

		// Admin endpoints
		[HttpGet]
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> GetAllOrders()
		{
			var orders = await _orderService.GetAllOrdersAsync();
			return Ok(orders);
		}

		[HttpPut("{orderId}/status")]
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] int status)
		{
			var updatedOrder = await _orderService.UpdateOrderStatusAsync(orderId, status);
			return Ok(updatedOrder);
		}
	}
}
