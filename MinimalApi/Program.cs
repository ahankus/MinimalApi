using MassTransit;
using MinimalApi.Consumer;
using MinimalApi.Handlers;
using MinimalApi.Interfaces;
using MinimalApi.Repositories;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var url = new MongoUrl("mongodb://127.0.0.1:27017/MinimalApiDb");
var client = new MongoClient();
var database = client.GetDatabase(url.DatabaseName);

builder.Services.AddSingleton<IMongoClient>(client);
builder.Services.AddSingleton(database);
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IAccountNumberRepository, AccountNumberRepository>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AccountNumberDeletedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("rabbitmq://localhost/"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();

app.MapGet("/api/customers", CustomerHandlers.GetCustomersHandler).WithTags("Customers");
app.MapGet("/api/customers/{id}", CustomerHandlers.GetCutomerByIdHandler).WithTags("Customers");
app.MapPost("/api/customers", CustomerHandlers.AddCustomerHandler).WithTags("Customers");
app.MapDelete("/api/customers/{id}", CustomerHandlers.DeleteCustomerHandler).WithTags("Customers");

app.MapGet("/api/accountNumbers", AccountNumberHandlers.GetAccountNumbersHandler).WithTags("AccountNumbers");
app.MapGet("/api/accountNumbers/{number}", AccountNumberHandlers.GetAccountNumberByNumberHandler).WithTags("AccountNumbers");
app.MapPost("/api/accountNumbers", AccountNumberHandlers.AddAccountNumberHandler).WithTags("AccountNumbers");
app.MapDelete("/api/accountNumbers/{id}", AccountNumberHandlers.DeleteAccountNumberHandler).WithTags("AccountNumbers");

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

app.Run();

public partial class Program;