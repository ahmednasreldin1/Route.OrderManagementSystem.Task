using Route.OrderManagementSystem.Core.Models;
using Route.OrderManagementSystem.Core.Specifications;

namespace Route.OrderManagementSystem.Core.Contracts.Repositories
{
	public interface IGenericRepository<T> where T : ModelBase
	{
		Task<T?> GetAsync(int id);
		Task<IReadOnlyList<T>> GetAllAsync();
		Task<T?> GetWithSpecAsync(ISpecifications<T> spec);
		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
		Task<int> GetCountAsync(ISpecifications<T> spec);

		void Add(T entity);

		void Update(T entity);

		void Delete(T entity);

	}
}
