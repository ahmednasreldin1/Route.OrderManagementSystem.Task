using Route.OrderManagementSystem.Core.Contracts.Repositories;
using Route.OrderManagementSystem.Core.Contracts.UnitOfWork;
using Route.OrderManagementSystem.Core.Data;
using Route.OrderManagementSystem.Core.Models;
using Route.OrderManagementSystem.Infrastructure.Generic_Repository;
using System.Collections;

namespace Route.OrderManagementSystem.Infrastructure.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{

		private Hashtable _repositories;
		private readonly OrderManagementDbContext _dbContext;

		public UnitOfWork(OrderManagementDbContext dbContext)
		// Ask CLR for Creating Object from DbContext Implicitly
		{
			_repositories = new Hashtable();
			_dbContext = dbContext;
		}

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : ModelBase
		{
			var key = typeof(TEntity).Name; 
			if(!_repositories.ContainsKey(key))
			{
				var repository = new GenericRepository<TEntity>(_dbContext);
				_repositories.Add(key, repository);
			}

			return _repositories[key] as IGenericRepository<TEntity>;
		}

		public async Task<int> CompleteAsync()
			=> await _dbContext.SaveChangesAsync();

		public async ValueTask DisposeAsync()
			=> await _dbContext.DisposeAsync();

	}
}
