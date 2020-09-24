using ECommerce.Api.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider _productsProvider;

        public ProductsController(IProductsProvider productsProvider)
        {
            this._productsProvider = productsProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var result = await _productsProvider.GetProductsAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Products);
            }

            return NotFound();
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProductByIdAsync(int Id)
        {
            var result = await _productsProvider.GetProductByIdAsync(Id);

            if (result.IsSuccess)
            {
                return Ok(result.Product);
            }

            return NotFound();
        }
    }
}
