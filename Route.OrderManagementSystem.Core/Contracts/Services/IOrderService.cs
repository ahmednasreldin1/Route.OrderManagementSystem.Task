using Route.OrderManagementSystem.Core.Models.Order;

namespace Route.OrderManagementSystem.Core.Contracts.Services
{
	public interface IOrderService
	{
		Task<Order?> CreateOrderAsync(Order order);

		Task<Order> UpdateOrderStatusAsync(int orderId, int status);

		Task<IReadOnlyList<Order>> GetAllOrdersAsync();

		Task<Order?> GetOrderByIdForUserAsync(string customerEmail, int orderId);

	}
}
