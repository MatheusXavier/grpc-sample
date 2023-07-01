using Basket.API.Model;

using Grpc.Core;

using ProductApi;

namespace Basket.API.Services;

public class ProductService : IProductService
{
    private readonly ProductGrpc.ProductGrpcClient _client;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        ProductGrpc.ProductGrpcClient client,
        ILogger<ProductService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<ProductItem?> GetProductAsync(int productId)
    {
        GetProductRequest request = new() { Id = productId };

        try
        {
            GetProductResponse response = await _client.GetProductAsync(request);

            return new ProductItem(
                response.Id,
                response.Name,
                response.Description,
                response.Price);
        }
        catch (RpcException e)
        {
            _logger.LogWarning(e, "ERROR - Parameters: {@parameters}", request);

            return null;
        }
    }
}