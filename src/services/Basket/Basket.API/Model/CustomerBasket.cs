namespace Basket.API.Model;

public class CustomerBasket
{
    public Guid BuyerId { get; set; }

    public List<BasketItem> Items { get; set; } = new List<BasketItem>();

    public CustomerBasket(Guid customerId)
    {
        BuyerId = customerId;
    }

    public CustomerBasket()
    {

    }
}