using Microsoft.EntityFrameworkCore;

using Product.API.Extensions;
using Product.API.Infrastructure;
using Product.API.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProductContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionString"]);
});

WebApplication app = builder.Build();

app.MigrateDbContext<ProductContext>((context, services) =>
{
    IWebHostEnvironment env = services.GetRequiredService<IWebHostEnvironment>();
    ILogger<ProductContextSeed> logger = services.GetRequiredService<ILogger<ProductContextSeed>>();

    new ProductContextSeed()
        .SeedAsync(context, env, logger)
        .Wait();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<ProductService>();

app.Run();
