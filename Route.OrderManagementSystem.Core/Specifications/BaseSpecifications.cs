﻿using Route.OrderManagementSystem.Core.Models;
using System.Linq.Expressions;

namespace Route.OrderManagementSystem.Core.Specifications
{
	public class BaseSpecifications<T> : ISpecifications<T> where T : ModelBase
	{
        public Expression<Func<T, bool>> Criteria { get; set; } = null!;
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
		public Expression<Func<T, object>> OrderBy { get; set; } = null!;
		public Expression<Func<T, object>> OrderByDesc { get; set; } = null!;
		public int Skip { get; set; } = 0;
		public int Take { get; set; } = 0;
		public bool IsPaginationEnabled { get; set; } = false;

		public BaseSpecifications()
        {
            //Criteria = null;
		}

        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression; 
		}

		public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
		{
			OrderBy = orderByExpression; 
		}
		public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
		{
			OrderByDesc = orderByDescExpression; 
		}

		public void ApplyPagination(int skip, int take)
		{
			IsPaginationEnabled = true;
			Skip = skip; 
			Take = take; 
		}
	}
}
