namespace Basket.API.Model;

public record CustomerBasket(
    Guid BuyerId,
    List<BasketItem> Items)
{
    public double Total => Items.Sum(p => p.Total);
}