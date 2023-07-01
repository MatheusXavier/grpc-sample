using Basket.API.Model;

namespace Basket.API.Services;

public interface IProductService
{
    Task<ProductItem?> GetProductAsync(int productId);
}