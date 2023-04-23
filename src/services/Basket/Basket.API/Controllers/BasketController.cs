using Basket.API.Model;

using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;

    public BasketController(IBasketRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("api/v1/basket/{id:guid}")]
    public async Task<IActionResult> GetBasketByIdAsync(Guid id)
    {
        CustomerBasket? basket = await _repository.GetBasketAsync(id);

        return Ok(basket ?? new CustomerBasket(id));
    }

    [HttpPost("api/v1/basket")]
    public async Task<IActionResult> UpdateBasketAsync(CustomerBasket value)
    {
        return Ok(await _repository.UpdateBasketAsync(value));
    }

    [HttpDelete("api/v1/basket/{id:guid}")]
    public async Task<IActionResult> DeleteBasketByIdAsync(Guid id)
    {
        await _repository.DeleteBasketAsync(id);

        return Ok();
    }
}