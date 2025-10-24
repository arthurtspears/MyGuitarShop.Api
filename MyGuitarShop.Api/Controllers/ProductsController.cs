


using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Data.Ado.Repositories;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(
        ILogger<ProductsController> logger,
        ProductRepo repo) 
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await repo.GetAllProductsAsync();

                return Ok(products.Select(p => p.ProductName));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Products");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
