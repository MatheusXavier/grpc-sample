using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Product.API.Infrastructure;

namespace Product.API.Controllers;

[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductContext _context;

    public ProductsController(ProductContext context)
    {
        _context = context;
    }

    [HttpGet("api/v1/products")]
    public async Task<IActionResult> GetProductsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        return Ok(await _context.Products
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync());
    }
}