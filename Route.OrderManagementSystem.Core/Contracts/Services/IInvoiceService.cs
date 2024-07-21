using Route.OrderManagementSystem.Core.Models.Invoice;
using Route.OrderManagementSystem.Core.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.OrderManagementSystem.Core.Contracts.Services
{
	public interface IInvoiceService
	{
		Task<IReadOnlyList<Invoice>> GetAllInvoicesAsync();
		Task<Invoice?> GetInvoiceByIdAsync(int invoiceId);
		Task<int> CreateInvoiceAsync(Order order);
	}
}
