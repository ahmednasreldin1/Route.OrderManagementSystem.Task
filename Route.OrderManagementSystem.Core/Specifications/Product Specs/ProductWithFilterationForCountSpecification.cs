using Route.OrderManagementSystem.Core.Models.Product;

namespace Route.OrderManagementSystem.Core.Specifications.Product_Specs
{
	public class ProductWithFilterationForCountSpecification : BaseSpecifications<Product>
	{
        public ProductWithFilterationForCountSpecification(ProductSpecParams specParams)
            :base(P =>
					(string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) 
			)
        {
            
        }
    }
}
