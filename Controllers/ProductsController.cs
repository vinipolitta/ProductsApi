using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
    public IEnumerable<ReadProductDto> GetProducts([FromQuery] int skip = 0, [FromQuery] int take = 5)
    {

        return _mapper.Map<List<ReadProductDto>>(_context.Products.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult GetProductsById(int id)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound("Produto nao encontrado");
        var productDto = _mapper.Map<ReadProductDto>(product);
        return Ok(productDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] UpdateProductDto productDto)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound("Erro ao fazer o update");

        _mapper.Map(productDto, product);
        _context.SaveChanges();

        return Ok("Atualizado com sucesso");
        // return NoContent();

    }

    [HttpPatch("{id}")]
    public IActionResult UpdateProductPath(int id, [FromBody] JsonPatchDocument<UpdateProductDto> patch)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound("Erro ao fazer o update");


        var productFroUpdate = _mapper.Map<UpdateProductDto>(product);
        // return NoContent();
        patch.ApplyTo(productFroUpdate, ModelState);
        if (!TryValidateModel(productFroUpdate))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(productFroUpdate, product);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound("Erro ao fazer o delete");

        _context.Remove(product);
        _context.SaveChanges();


        return NoContent();
        // return NoContent();

    }




}