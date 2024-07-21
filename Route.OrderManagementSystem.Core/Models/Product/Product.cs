namespace Route.OrderManagementSystem.Core.Models.Product
{
    public class Product : ModelBase
	{
		public string Name { get; set; } = null!;
		public decimal Price { get; set; }
		public int Stock { get; set; }
    }
}
