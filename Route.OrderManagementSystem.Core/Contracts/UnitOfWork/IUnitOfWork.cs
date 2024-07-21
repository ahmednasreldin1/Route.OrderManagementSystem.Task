using Route.OrderManagementSystem.Core.Contracts.Repositories;
using Route.OrderManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.OrderManagementSystem.Core.Contracts.UnitOfWork
{
	public interface IUnitOfWork : IAsyncDisposable
	{
		IGenericRepository<TEntity> Repository<TEntity>() where TEntity :  ModelBase;

		Task<int> CompleteAsync();
	}
}
