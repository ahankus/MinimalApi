using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Consumer;
using MinimalApi.Interfaces;
using MinimalApi.Repositories;
using MongoDB.Driver;
using Testcontainers.RabbitMq;

namespace IntegrationTests.TestFixtures;

public class MinimalApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName;
    public MinimalApiWebApplicationFactory()
    {
        MongoRunnerProvider.Get();
        
        _dbName = new Random().Next(10, 1000).ToString();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                {
                    "MongoDBSettings:ConnectionString", "mongodb://127.0.0.1:27017/MinimalApiDb" + _dbName
                }
            });
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(sp =>
            {
                var mongo = sp.GetRequiredService<IMongoClient>();
                return mongo.GetDatabase("MinimalApiDb" + _dbName);
            });

            services.AddMassTransitTestHarness();
            services.AddTransient<IAccountNumberRepository, AccountNumberRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
        });
    }
}