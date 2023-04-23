using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using Product.API.Infrastructure.EntityConfigurations;
using Product.API.Model;

namespace Product.API.Infrastructure;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }

    public DbSet<ProductItem> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductItemEntityTypeConfiguration());
    }
}

public class ProductContextDesignFactory : IDesignTimeDbContextFactory<ProductContext>
{
    public ProductContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<ProductContext> optionsBuilder = new DbContextOptionsBuilder<ProductContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Initial Catalog=grpc-sample-product-db;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new ProductContext(optionsBuilder.Options);
    }
}