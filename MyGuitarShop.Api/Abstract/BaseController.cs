using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Api.Mappers;
using MyGuitarShop.Common.Interfaces;

namespace MyGuitarShop.Api.Abstract
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TDto, TEntity>(
        IRepository<TEntity> repo,
        ILogger<BaseController<TDto, TEntity>> logger
        ) : ControllerBase where TEntity : class, new()
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var entities = await repo.GetAllAsync();

                return entities.Any() ? Ok(entities) : NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching entities");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var entity = await repo.GetByIdAsync(id);

                if (entity == null)
                    return NotFound($"Entity with id {id} not found");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Entity");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(TDto dto)
        {
            try
            {
                var entity = AutoReflectionMapper.Map<TDto, TEntity>(dto) 
                    ?? throw new InvalidOperationException("Unable to map Dto to Entity");

                var entitiesCreated = await repo.InsertAsync(entity);

                return Ok($"{entitiesCreated} entities created.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding new entities");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, TDto dto)
        {
            try
            {
                if (await repo.GetByIdAsync(id) == null)
                    return NotFound($"Entity with id {id} not found");

                var entity = AutoReflectionMapper.Map<TDto, TEntity>(dto)
                             ?? throw new InvalidOperationException("Unable to map Dto to Entity");

                var numberUpdated = await repo.UpdateAsync(id, entity);

                return Ok($"{numberUpdated} entities updated.");
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
                if (await repo.GetByIdAsync(id) == null)
                    return NotFound($"Entity with id {id} not found");

                var numberDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberDeleted} products deleted.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting Product");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
