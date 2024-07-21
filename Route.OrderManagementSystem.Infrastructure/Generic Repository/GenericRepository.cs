using Microsoft.EntityFrameworkCore;
using Route.OrderManagementSystem.Core.Contracts.Repositories;
using Route.OrderManagementSystem.Core.Data;
using Route.OrderManagementSystem.Core.Models;
using Route.OrderManagementSystem.Core.Specifications;

namespace Route.OrderManagementSystem.Infrastructure.Generic_Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
	{
		private readonly OrderManagementDbContext _dbContext;

		public GenericRepository(OrderManagementDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<T?> GetAsync(int id) 
			=> await _dbContext.Set<T>().FindAsync(id);
		

		public async Task<IReadOnlyList<T>> GetAllAsync() 
			=> await _dbContext.Set<T>().ToListAsync();
		
		public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec) 
			=> await ApplySpecifications(spec).FirstOrDefaultAsync();
		
		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec) 
			=> await ApplySpecifications(spec).ToListAsync();
		
		public async Task<int> GetCountAsync(ISpecifications<T> spec) 
			=> await ApplySpecifications(spec).CountAsync();
		

		private IQueryable<T> ApplySpecifications(ISpecifications<T> spec) 
			=> SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
		

		public void Add(T entity)
			=> _dbContext.Set<T>().Add(entity);

		public void Update(T entity)
			=> _dbContext.Set<T>().Update(entity);

		public void Delete(T entity)
			=> _dbContext.Set<T>().Remove(entity);
	}
}
