using Route.OrderManagementSystem.Core.Models.Order;
using System.Linq.Expressions;

namespace Route.OrderManagementSystem.Core.Specifications.Order_Specs
{
	public class OrderSpecifications : BaseSpecifications<Order>
	{
		public OrderSpecifications()
		{
			Includes.Add(O => O.Items);
		}

		public OrderSpecifications(Expression<Func<Order, bool>> criteria)
            :base(criteria)
        {
            Includes.Add(O => O.Items);
		}


    }
}
