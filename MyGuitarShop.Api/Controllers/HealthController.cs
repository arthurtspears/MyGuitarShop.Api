

using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Data.EFCore.Context;

namespace MyGuitarShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController(
        ILogger<HealthController> logger,
        SqlConnectionFactory sqlConnectionFactory,
        MyGuitarShopContext dbContext,
        IMongoClient mongoClient) 
        : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok("Healthy");
            }
            catch (Exception)
            {
                logger.LogWarning("Health check failed unreasonably.");

                return StatusCode(503, "Unhealthy");
            }
        }

        [HttpGet("db/ado")]
        public IActionResult GetDbHealth()
        {
            try
            {
                using var connection = sqlConnectionFactory.OpenSqlConnection();

                return Ok(new { Message = "Connection successful!", connection.Database });
            }
            catch (Exception)
            {
                logger.LogCritical("Database health check failed.");

                return StatusCode(503, "Database Unhealthy");
            }
        }

        [HttpGet("db/efcore")]
        public async Task<IActionResult> GetDbContextHealthAsync()
        {
            try
            {
                if(!await dbContext.Database.CanConnectAsync())
                    throw new Exception("Cannot connect to database via EF Core DbContext.");

                return Ok(new { Message = "Connection successful!", dbContext.Database });
            }
            catch (Exception)
            {
                logger.LogCritical("Database health check failed.");

                return StatusCode(503, "Database Unhealthy");
            }
        }

        [HttpGet("db/mongo")]
        public async Task<IActionResult> GetMongoHealthAsync()
        {
            try
            {
                var response = await mongoClient.ListDatabaseNamesAsync();
                var databaseNames = await response.ToListAsync() ?? [];
                if (databaseNames.Count == 0)
                    throw new Exception("Cannot connect to Mongo Database.");

                return Ok(new { Message = "Connection successful!", databaseNames });
            }
            catch (Exception)
            {
                logger.LogCritical("Mongo Database health check failed.");

                return StatusCode(503, "Database Unhealthy");
            }
        }
    }
}
