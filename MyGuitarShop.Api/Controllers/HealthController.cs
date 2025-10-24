

using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Data.Ado.Factories;

namespace MyGuitarShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController(
        ILogger<HealthController> logger,
        SqlConnectionFactory sqlConnectionFactory) 
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

        [HttpGet("db")]
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
    }
}
