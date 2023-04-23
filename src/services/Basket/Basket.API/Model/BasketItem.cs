namespace Basket.API.Model;

public class BasketItem
{
    public Guid Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }
}
