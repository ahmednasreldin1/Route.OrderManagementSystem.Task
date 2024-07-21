using Route.OrderManagementSystem.Core.Contracts.Services;
using Route.OrderManagementSystem.Core.Contracts.UnitOfWork;
using Route.OrderManagementSystem.Core.Models.Invoice;
using Route.OrderManagementSystem.Core.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.OrderManagementSystem.Application.Services.InvoiceService
{
	public class InvoiceService : IInvoiceService
	{
		private readonly IUnitOfWork _unitOfWork;

		public InvoiceService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		public async Task<int> CreateInvoiceAsync(Order order)
		{
			var invoice = new Invoice
			{
				OrderId = order.Id,
				InvoiceDate = DateTimeOffset.UtcNow,
				TotalAmount = order.TotalAmount
			};

			_unitOfWork.Repository<Invoice>().Add(invoice);
			return await _unitOfWork.CompleteAsync();
		}


		public async Task<IReadOnlyList<Invoice>> GetAllInvoicesAsync()
		{
			return await _unitOfWork.Repository<Invoice>().GetAllAsync();
		}

		public async Task<Invoice?> GetInvoiceByIdAsync(int invoiceId)
		{
			return await _unitOfWork.Repository<Invoice>().GetAsync(invoiceId);
		}

	}
}
