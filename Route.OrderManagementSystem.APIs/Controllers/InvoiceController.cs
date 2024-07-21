using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Route.OrderManagementSystem.APIs.Errors;
using Route.OrderManagementSystem.Core.Contracts.Services;
using Route.OrderManagementSystem.Core.Models.Invoice;

namespace Route.OrderManagementSystem.APIs.Controllers
{

	public class InvoiceController : BaseApiController
	{
		private readonly IInvoiceService _invoiceService;

		public InvoiceController(IInvoiceService invoiceService)
		{
			_invoiceService = invoiceService;
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllInvoices()
		{
			var invoices = await _invoiceService.GetAllInvoicesAsync();
			return Ok(invoices);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("{invoiceId}")]
		[ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Invoice>> GetInvoiceById(int invoiceId)
		{
			var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
			if (invoice == null)
			{
				return NotFound(new ApiResponse(404));
			}
			return Ok(invoice);
		}
	}
}
