using Route.OrderManagementSystem.Core.Models.Order;
using System.ComponentModel.DataAnnotations;

namespace Route.OrderManagementSystem.APIs.Dtos
{
	public class OrderDto
	{
		[Required]
		public string PaymentMethod { get; set; } = null!;
		[Required]
		public ICollection<OrderItem> Items { get; set; } = null!;

	}
}
