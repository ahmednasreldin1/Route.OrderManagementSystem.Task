using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Route.OrderManagementSystem.APIs.Errors;
using Route.OrderManagementSystem.APIs.Helpers;
using Route.OrderManagementSystem.Core.Contracts.Services;
using Route.OrderManagementSystem.Core.Models.Product;
using Route.OrderManagementSystem.Core.Specifications.Product_Specs;
namespace Route.OrderManagementSystem.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IProductService _productService;	

		public ProductsController(
			IProductService productService)
		{
			_productService = productService;
		}

		[Cached(600)]
		[HttpGet] // GET : /api/Products
		public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
		{
			var products = await _productService.GetProductsAsync(specParams);

			var count = await _productService.GetCountAsync(specParams);

			return Ok(new Pagination<Product>(specParams.PageIndex, specParams.PageSize, count, products));
		}

		// /api/Products/1
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await _productService.GetProductAsync(id);

			if (product is null)
				return NotFound(new ApiResponse(404)); // 404

			return Ok(product); // 200
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<Product>> CreateProduct(Product product)
		{
			var createdProduct = await _productService.CreateProductAsync(product);
			return Ok(createdProduct);
		}

		[HttpPut("{productId}")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest	)]
		public async Task<ActionResult<Product>> UpdateProduct(int productId, Product product)
		{
			if (productId != product.Id)
			{
				return BadRequest(new ApiResponse(400, "Product ID mismatch"));
			}

			var updatedProduct = await _productService.UpdateProductAsync(productId, product);
			return Ok(updatedProduct);
		}
	}

}
