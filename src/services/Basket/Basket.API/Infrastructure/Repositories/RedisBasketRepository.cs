using Basket.API.Model;

using StackExchange.Redis;

using System.Text.Json;

namespace Basket.API.Infrastructure.Repositories;

public class RedisBasketRepository : IBasketRepository
{
    private readonly IDatabase _database;

    public RedisBasketRepository(ConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<CustomerBasket?> GetBasketAsync(Guid customerId)
    {
        RedisValue data = await _database.StringGetAsync(customerId.ToString());

        if (data.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<CustomerBasket>(data!, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
    {
        bool created = await _database.StringSetAsync(basket.BuyerId.ToString(), JsonSerializer.Serialize(basket));

        if (!created)
        {
            return null;
        }

        return await GetBasketAsync(basket.BuyerId);
    }
}
