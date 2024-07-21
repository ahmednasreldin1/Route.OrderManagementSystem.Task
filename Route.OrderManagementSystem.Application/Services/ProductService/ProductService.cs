using Route.OrderManagementSystem.Core.Contracts.Services;
using Route.OrderManagementSystem.Core.Contracts.UnitOfWork;
using Route.OrderManagementSystem.Core.Models.Product;
using Route.OrderManagementSystem.Core.Specifications.Product_Specs;

namespace Route.OrderManagementSystem.Application.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams)
        {
            var spec = new ProductSpecifications(specParams);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            return products;
        }
       
        public async Task<Product?> GetProductAsync(int productId)
        {
            var spec = new ProductSpecifications(productId);

            var product = await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);

            return product;
        }

        public async Task<int> GetCountAsync(ProductSpecParams specParams)
        {
            var countSpec = new ProductWithFilterationForCountSpecification(specParams);

            var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);

            return count;
        }

		public async Task<Product> CreateProductAsync(Product product)
		{
			_unitOfWork.Repository<Product>().Add(product);
			await _unitOfWork.CompleteAsync();
            return product;
		}

		public async Task<Product> UpdateProductAsync(int productId, Product product)
		{
            var productsRepo = _unitOfWork.Repository<Product>();

			var existingProduct = await productsRepo.GetAsync(productId);

			if (existingProduct == null)
				throw new KeyNotFoundException("Product not found");
			

			existingProduct.Name = product.Name;
			existingProduct.Price = product.Price;
			existingProduct.Stock = product.Stock;

			productsRepo.Update(existingProduct);
			await _unitOfWork.CompleteAsync();

            return product;
		}

	}
}
