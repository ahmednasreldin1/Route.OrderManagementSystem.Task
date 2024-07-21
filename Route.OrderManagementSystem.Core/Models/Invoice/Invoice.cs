namespace Route.OrderManagementSystem.Core.Models.Invoice
{
    public class Invoice : ModelBase
	{
        public int OrderId { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
