namespace Basket.API.Model;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetBasketAsync(Guid customerId);

    Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
}