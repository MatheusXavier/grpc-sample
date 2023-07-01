using Grpc.Core;

using Microsoft.EntityFrameworkCore;

using Product.API.Infrastructure;
using Product.API.Model;

using ProductApi;

namespace Product.API.Services;

public class ProductService : ProductGrpc.ProductGrpcBase
{
    private readonly ProductContext _context;

    public ProductService(ProductContext context)
    {
        _context = context;
    }

    public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        ProductItem? product = await _context.Products
            .Where(p => p.Id == request.Id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (product is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Could not found product with id: {request.Id}"));
        }

        if (!product.Active)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Product with id: {request.Id} is disabled"));
        }

        return new GetProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
        };
    }
}
