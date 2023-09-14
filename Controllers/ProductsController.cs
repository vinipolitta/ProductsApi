using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Data;
using ProductsApi.Data.Dtos;
using ProductsApi.Models;

namespace ProductsApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private ProductContext _context;
    private IMapper _mapper;

    public ProductsController(ProductContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AddProduct([FromBody] CreateProductDto productDto)
    {
        Product product = _mapper.Map<Product>(productDto);
        _context.Products.Add(product);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetProductsById), new { id = product.Id }, product);
    }

    [HttpGet]
    public IEnumerable<Product> RecoveryProducts([FromQuery] int skip = 0, [FromQuery] int take = 5)
    {

        return _context.Products.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductsById(int id)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound("Produto nao encontrado");
        return Ok(product);
    }



}