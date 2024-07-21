using Route.OrderManagementSystem.Core.Models.Product;
using Route.OrderManagementSystem.Core.Specifications.Product_Specs;

namespace Route.OrderManagementSystem.Core.Contracts.Services
{
	public interface IProductService
	{
		Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams);

		Task<Product?> GetProductAsync(int productId);

		Task<int> GetCountAsync(ProductSpecParams specParams);

		Task<Product> CreateProductAsync(Product product);
		Task<Product> UpdateProductAsync(int productId, Product product);

	}
}
