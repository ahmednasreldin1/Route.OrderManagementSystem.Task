﻿using Microsoft.EntityFrameworkCore;
using Route.OrderManagementSystem.Core.Models;
using Route.OrderManagementSystem.Core.Specifications;

namespace Route.OrderManagementSystem.Infrastructure.Generic_Repository
{
	internal static class SpecificationsEvaluator<TEntity> where TEntity : ModelBase
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
		{
			var query = inputQuery;

			if(spec.Criteria is not null) 
				query = query.Where(spec.Criteria);

			if (spec.OrderBy is not null) 
				query = query.OrderBy(spec.OrderBy);

			else if (spec.OrderByDesc is not null) 
				query = query.OrderByDescending(spec.OrderByDesc);

			if (spec.IsPaginationEnabled)
				query = query.Skip(spec.Skip).Take(spec.Take);

			

			query = spec.Includes.Aggregate(query, (currentQuery, nextInclude) => currentQuery.Include(nextInclude));

			
			return query;
		}
	}
}
