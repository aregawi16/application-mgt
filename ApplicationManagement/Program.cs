using System.Collections.Concurrent;
using System.ComponentModel;
using ApplicationManagement.Services;
using Microsoft.Azure.Cosmos;
using Container = Microsoft.Azure.Cosmos.Container;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddSingleton((provider) =>
{
    var endpointUri = configuration["CosmosDb:EndpointUri"];
    var primaryKey = configuration["CosmosDb:Key"];
    var databaseName = configuration["CosmosDb:DatabaseName"];

    var cosmosClientOptions = new CosmosClientOptions
    {
        ApplicationName = databaseName,
        ConnectionMode = ConnectionMode.Gateway,

        //ServerCertificateCustomValidationCallback = (request, certificate, chain) =>
        //{
        //    // Always return true to ignore certificate validation errors
        //    return true; //not for production
        //}
    };

    var loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });

    var cosmosClient = new CosmosClient(endpointUri, primaryKey, cosmosClientOptions);
    
    return cosmosClient;
});
builder.Services.AddSingleton<CosmosDbService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

   

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
