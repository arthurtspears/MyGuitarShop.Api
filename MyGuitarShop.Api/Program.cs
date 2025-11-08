using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Data.Ado.Repositories;
using System.Diagnostics;
using MyGuitarShop.Data.EFCore.Context;
using MyGuitarShop.Data.EFCore.Repositories;

namespace MyGuitarShop.Api
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add logging to the container.
                AddLogging(builder);

                // Add services to the container.
                AddServices(builder);

                if (builder.Environment.IsDevelopment())
                {
                    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                    builder.Services.AddEndpointsApiExplorer();
                    builder.Services.AddSwaggerGen();
                }

                builder.Host.UseDefaultServiceProvider(options =>
                {
                    options.ValidateScopes = true;
                    options.ValidateOnBuild = true;
                });

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapGet("/", () => "API running in Development");
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                ConfigureApplication(app);

                await app.RunAsync();
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached) Debugger.Break();

                Console.WriteLine(ex.Message);
            }
        }

        private static void AddLogging(WebApplicationBuilder builder)
        {
            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging
                    .AddFilter("Microsoft", LogLevel.Information)
                    .AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information)
                    .AddConsole();
            });

            builder.Services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.RequestPath
                                        | HttpLoggingFields.RequestMethod
                                        | HttpLoggingFields.ResponseStatusCode;
            });
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("MyGuitarShop") 
                ?? throw new InvalidOperationException("MyGuitarShop connection string not found.");

            builder.Services.AddSingleton(new SqlConnectionFactory(connectionString)); //ADO.NET Specific

            builder.Services.AddScoped<MyGuitarShop.Data.Ado.Repositories.ProductRepo>();

            builder.Services.AddDbContextFactory<MyGuitarShopContext>(options =>
                options.UseSqlServer(connectionString)); //EFCore Specific

            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.ProductRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.CategoryRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.AddressRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.CustomerRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.OrderRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.OrderItemRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.AdministratorRepository>();

            builder.Services.AddControllers();
        }

        private static void ConfigureApplication(WebApplication app)
        {
            app.UseHttpLogging();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}