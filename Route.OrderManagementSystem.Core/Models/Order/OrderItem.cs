namespace Route.OrderManagementSystem.Core.Models.Order
{
    public class OrderItem : ModelBase
	{
		public int ProductId { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public decimal Discount { get; set; }
	}
}
