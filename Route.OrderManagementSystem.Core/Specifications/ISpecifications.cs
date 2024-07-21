using Route.OrderManagementSystem.Core.Models;
using System.Linq.Expressions;

namespace Route.OrderManagementSystem.Core.Specifications
{
	public interface ISpecifications<T> where T : ModelBase
	{
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
