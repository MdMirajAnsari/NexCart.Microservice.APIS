using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NexCart.Products.DTO;
using NexCart.Products.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexCart.ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            List<ProductResponse?> products = await _productsService.GetProducts();
            return Ok(products);
        }

        [HttpGet("search/product-id/{productID:guid}")]
        public async Task<IActionResult> GetProductById(Guid productID)
        {
            ProductResponse? product = await _productsService.GetProductByCondition(k => k.ProductID == productID);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("search/{searchString}")]
        public async Task<IActionResult> Search(string searchString)
        {
            List<ProductResponse?> productsByProductName =
                await _productsService.GetProductsByCondition(k => k.ProductName != null && k.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase));

            List<ProductResponse?> productsByCategory =
                await _productsService.GetProductsByCondition(k => k.Category != null && k.Category.Contains(searchString, StringComparison.OrdinalIgnoreCase));

            var products = productsByProductName.Union(productsByCategory);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromServices] IValidator<ProductAddRequest> productAddRequestValidator, [FromBody] ProductAddRequest productAddRequest)
        {
            ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors =
                    validationResult.Errors.GroupBy(k => k.PropertyName).ToDictionary(l => l.Key, m => m.Select(err => err.ErrorMessage).ToArray());
                return ValidationProblem(new ValidationProblemDetails(errors));
            }

            var addedProductResponse = await _productsService.AddProduct(productAddRequest);
            if (addedProductResponse != null)
            {
                return Created($"/api/products/search/product-id/{addedProductResponse.ProductID}", addedProductResponse);
            }
            else
            {
                return Problem("Failed to add product");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromServices] IValidator<ProductUpdateRequest> productUpdateRequestValidator, [FromBody] ProductUpdateRequest productUpdateRequest)
        {
            ValidationResult validationResult = await productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors =
                    validationResult.Errors.GroupBy(k => k.PropertyName).ToDictionary(l => l.Key, m => m.Select(err => err.ErrorMessage).ToArray());
                return ValidationProblem(new ValidationProblemDetails(errors));
            }

            var updatedProductResponse = await _productsService.UpdateProduct(productUpdateRequest);
            if (updatedProductResponse != null)
            {
                return Ok(updatedProductResponse);
            }
            else
            {
                return Problem("Failed to update product");
            }
        }

        [HttpDelete("{ProductID:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid ProductID)
        {
            bool isDeleted = await _productsService.DeleteProduct(ProductID);
            if (isDeleted)
            {
                return Ok(true);
            }
            else
            {
                return Problem("Failed to delete product");
            }
        }
    }
}
