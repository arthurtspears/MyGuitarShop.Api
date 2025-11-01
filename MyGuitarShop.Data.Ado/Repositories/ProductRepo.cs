using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Common.Dtos;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;

namespace MyGuitarShop.Data.Ado.Repositories
{
    public class ProductRepo(
        ILogger<ProductRepo> logger,
        SqlConnectionFactory connectionFactory)
    {
        public async Task<ProductEntity?> FindProductByProductCodeAsync(ProductDto productDto)
        {
            ProductEntity? product = null;
            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();
                await using var cmd = new SqlCommand("SELECT * FROM Products WHERE ProductCode = @ProductCode", conn);
                cmd.Parameters.AddWithValue("@ProductCode", productDto.ProductCode);
                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    product = new ProductEntity
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
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving product by ProductCode");
            }
            return product;
        }

        public async Task<ProductEntity?> GetProductByIdAsync(int id)
        {
            ProductEntity? product = null;
            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();
                await using var cmd = new SqlCommand("SELECT * FROM Products WHERE ProductID = @ProductID", conn);
                cmd.Parameters.AddWithValue("@ProductID", id);
                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    product = new ProductEntity
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
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving product by ID");
            }
            return product;
        }

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

        public async Task<int> InsertAsync(ProductDto product)
        {
            const string query = @"INSERT INTO Products (CategoryID, ProductCode, ProductName, Description, ListPrice, DiscountPercent, DateAdded)
                               VALUES (@CategoryID, @ProductCode, @ProductName, @Description, @ListPrice, @DiscountPercent, @DateAdded)";
            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                await using var cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                cmd.Parameters.AddWithValue("@ProductCode", product.ProductCode);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@ListPrice", product.ListPrice);
                cmd.Parameters.AddWithValue("@DiscountPercent", product.DiscountPercent);
                cmd.Parameters.AddWithValue("@DateAdded", DateTime.UtcNow);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error inserting product");
                return 0;
            }
        }

        public async Task<int> UpdateAsync(int id, ProductDto product)
        {
            const string query = @"UPDATE Products
                               SET ProductName = @ProductName, ListPrice = @ListPrice, DiscountPercent = @DiscountPercent
                               WHERE ProductID = @ProductID";
            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();
                await using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductID", id);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@ListPrice", product.ListPrice);
                cmd.Parameters.AddWithValue("@DiscountPercent", product.DiscountPercent);
                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating product");
                return 0;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            const string query = "DELETE FROM Products WHERE ProductID = @ProductID";
            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();
                await using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductID", id);
                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting product");
                return 0;
            }
        }
    }
}
