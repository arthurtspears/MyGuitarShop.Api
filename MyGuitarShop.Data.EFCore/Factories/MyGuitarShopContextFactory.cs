


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MyGuitarShop.Data.EFCore.Context;

namespace MyGuitarShop.Data.EFCore.Factories
{ 
    public class MyGuitarShopContextFactory : IDesignTimeDbContextFactory<MyGuitarShopContext>
    {
        public MyGuitarShopContext CreateDbContext(string[] args)
        {
            // Build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Get connection string
            var connectionString = configuration.GetConnectionString("MyGuitarShopMigrations");

            // Build options
            var optionsBuilder = new DbContextOptionsBuilder<MyGuitarShopContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Create and return context
            return new MyGuitarShopContext(optionsBuilder.Options);
        }
    }
}
