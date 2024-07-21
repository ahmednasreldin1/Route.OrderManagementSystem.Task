
namespace Route.OrderManagementSystem.Core.Models.Order
{
	public class Order : ModelBase
	{
		public int CustomerId { get; set; }
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
		public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
		public Customer.Customer Customer { get; set; } = null!;
	}
}
