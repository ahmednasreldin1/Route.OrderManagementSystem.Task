using Route.OrderManagementSystem.Core.Models.Customer;
using Route.OrderManagementSystem.Core.Models.Order;

namespace Route.OrderManagementSystem.Core.Contracts.Services
{
	public interface ICustomerService
	{
		Task<Customer> CreateCustomerAsync(Customer customer, string password);

		Task<IReadOnlyList<Order>> GetCustomerOrdersAsync(int customerId, string customerEmail);

	}
}
