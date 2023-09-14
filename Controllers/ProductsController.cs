using Microsoft.AspNetCore.Mvc;
using ProductsApi.Models;

namespace ProductsApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private static List<Product> products = new List<Product>();
    private static int id = 0;

    [HttpPost]
    public IActionResult AddProduct([FromBody] Product product)
    {
        product.Id = id++;
        products.Add(product);
        return CreatedAtAction(nameof(GetProductsById), new { id = product.Id }, product);
    }

    [HttpGet]
    public IEnumerable<Product> RecoveryProducts([FromQuery] int skip = 0, [FromQuery] int take = 5)
    {
        return products.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductsById(int id)
    {
        var product = products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound("Produto nao encontrado");
        return Ok(product);
    }



}