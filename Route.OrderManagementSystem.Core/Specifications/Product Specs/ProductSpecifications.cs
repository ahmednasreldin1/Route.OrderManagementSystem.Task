using Route.OrderManagementSystem.Core.Models.Product;

namespace Route.OrderManagementSystem.Core.Specifications.Product_Specs
{
	public class ProductSpecifications : BaseSpecifications<Product>
	{
        public ProductSpecifications(ProductSpecParams specParams)
            :base(P => 
					(string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search))
			)
		{


			if (!string.IsNullOrEmpty(specParams.Sort))
			{
				switch (specParams.Sort)
				{
					case "priceAsc":
						//OrderBy = P => P.Price;
						AddOrderBy(P => P.Price);
						break;
					case "priceDesc":
						//OrderByDesc = P => P.Price;
						AddOrderByDesc(P => P.Price);
						break;
					default:
						//OrderBy = P => P.Name;
						AddOrderBy(P => P.Name);
						break;

				}
			}
			else
				AddOrderBy(P => P.Name);


			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
		}


		public ProductSpecifications(int id)
            :base(P => P.Id == id)
        {
		}
    }
}
