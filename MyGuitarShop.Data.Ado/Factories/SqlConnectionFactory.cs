

using Microsoft.Data.SqlClient;

namespace MyGuitarShop.Data.Ado.Factories
{
    public class SqlConnectionFactory(string connectionString)
    {
        public SqlConnection OpenSqlConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
