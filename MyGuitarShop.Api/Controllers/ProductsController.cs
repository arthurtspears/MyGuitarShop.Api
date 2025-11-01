﻿using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.Dtos;
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
                
                return products.Any() ? Ok(products) : NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Products");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var product = await repo.GetProductByIdAsync(id);

                if(product == null)
                    return NotFound($"Product with id {id} not found");

                return Ok(product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Product");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(ProductDto newProduct)
        {
            try
            {
                var numberProductsCreated = await repo.InsertAsync(newProduct);

                return Ok($"{numberProductsCreated} new product created.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding new Product");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, ProductDto product)
        {
            try
            {
                if (await repo.GetProductByIdAsync(id) == null)
                    return NotFound($"Product with id {id} not found");

                var numberProductsUpdated = await repo.UpdateAsync(id, product);

                return Ok($"{numberProductsUpdated} new product created.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding new Product");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                if (await repo.GetProductByIdAsync(id) == null)
                    return NotFound($"Product with id {id} not found");

                var numberProductsUpdated = await repo.DeleteAsync(id);

                return Ok($"{numberProductsUpdated} products deleted.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting Product");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
