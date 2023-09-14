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


    /// <summary>
    /// Inicia o controller
    /// </summary>
    /// <param name="context">Context da database</param>
    /// <param name="mapper">mapper pra controlar os DTOS</param>

    public ProductsController(ProductContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um produto ao banco de dados
    /// </summary>
    /// <param name="productDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    public IActionResult AddProduct([FromBody] CreateProductDto productDto)
    {
        Product product = _mapper.Map<Product>(productDto);
        _context.Products.Add(product);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetProductsById), new { id = product.Id }, product);
    }

    /// <summary>
    /// Pega todos os produtos ao banco de dados
    /// </summary>
    /// <param name="skip">Objeto com os campos necessários para ler de um produtp</param>
    /// <param name="take">Objeto com os campos necessários para ler de um produtp</param>
    /// <returns>IEnumerable</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>

    [HttpGet]
    public IEnumerable<ReadProductDto> GetProducts([FromQuery] int skip = 0, [FromQuery] int take = 5)
    {

        return _mapper.Map<List<ReadProductDto>>(_context.Products.Skip(skip).Take(take));
    }


    /// <summary>
    /// Pega todos um produto por id no banco de dados
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para ler de um produtp</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>

    [HttpGet("{id}")]
    public IActionResult GetProductsById(int id)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound("Produto nao encontrado");
        var productDto = _mapper.Map<ReadProductDto>(product);
        return Ok(productDto);
    }

    /// <summary>
    /// Faz o update dos protudos na base de dados
    /// </summary>
    /// <param name="productDto">Objeto com os campos necessários para fazer o update de um produtp</param>
    /// <param name="id">id para filtrar o produto</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>

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

    /// <summary>
    /// Faz o update parcial do protudo na base de dados
    /// </summary>
    /// <param name="patch">Objeto com os campos necessários para fazer o update de um produto</param>
    /// <param name="id">id para filtrar o produto</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso inserção seja feita com sucesso</response>


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

    /// <summary>
    /// Faz o delete do protudo na base de dados
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso inserção seja feita com sucesso</response>


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