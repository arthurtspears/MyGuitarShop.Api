using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;

namespace MyGuitarShop.Data.Ado.Repositories
{
    public class ProductRepo(
        ILogger<ProductRepo> logger, 
        SqlConnectionFactory connectionFactory)
    {
        public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
        {
            var products = new List<ProductEntity>();
            
            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand("SELECT * FROM Products", conn);

                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var product = new ProductEntity
                    {
                        ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                        ProductCode = reader.GetString(reader.GetOrdinal("ProductCode")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                        DiscountPercent = reader.GetDecimal(reader.GetOrdinal("DiscountPercent")),
                        DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded"))
                    };
                    products.Add(product);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving product list");
            }
            return products;
        }
    }
}
