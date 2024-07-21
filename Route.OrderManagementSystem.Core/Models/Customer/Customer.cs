using Route.OrderManagementSystem.Core.Models.Identity;
using Route.OrderManagementSystem.Core.Models.Order;

namespace Route.OrderManagementSystem.Core.Models.Customer
{   
    public class Customer : ModelBase
    {
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
		public ICollection<Order.Order> Orders { get; set; } = new HashSet<Order.Order>();
    }
}
