using Basket.API.Infrastructure.Repositories;
using Basket.API.Model;
using Basket.API.Services;

using ProductApi;

using StackExchange.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddScoped<IProductService, ProductService>()
    .AddGrpcClient<ProductGrpc.ProductGrpcClient>((services, options) =>
    {
        options.Address = new Uri("https://localhost:7087");
    });


builder.Services.AddSingleton(sp =>
{
    string connectionString = builder.Configuration["ConnectionString"] ?? string.Empty;

    return ConnectionMultiplexer.Connect(connectionString);
});

builder.Services.AddTransient<IBasketRepository, RedisBasketRepository>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
