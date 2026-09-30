using AbmProductos.Models.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using AbmProductos.Services.Interfaces;

namespace AbmProductos.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }
    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = _service.GetAllProducts();
        return Ok(products);
    }
    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = _service.GetProductById(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }
    [HttpPost]
    public IActionResult CreateProduct([FromBody] ProductForCreateDto dto)
    {
        try
        {
            var created = _service.CreateProduct(dto);

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = created.Id },
                created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] ProductForUpdateDto dto)
    {
        if (_service.GetProductById(id) == null)
            return NotFound();

        try
        {
            _service.UpdateProduct(id, dto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        if (_service.GetProductById(id) == null)
            return NotFound();

        _service.DeleteProduct(id);
        return NoContent();
    }
    [HttpGet("search")]
    public IActionResult SearchProductsByName([FromQuery] string name)
    {
        var products = _service.SearchProductsByName(name);
        return Ok(products);
    }
    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        var stats = _service.GetStats();
        return Ok(stats);
    }
}