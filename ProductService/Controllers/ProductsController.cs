using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ProductService.Contracts;
using ProductService.Models;
using ProductService.Services;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _service;

    public ProductsController(IProductsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync()
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.GetAllAsync());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateAsync([FromBody] CreateProductContract contract)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        
        try
        {
            return StatusCode(StatusCodes.Status201Created, await _service.CreateAsync(contract));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetByIdAsync(Guid id)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.GetByIdAsync(id));
        }
        catch (ElementNotFoundException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (BadHttpRequestException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> UpdateAsync(Guid id, [FromBody] UpdateProductContract contract)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.UpdateAsync(id, contract));
        }
        catch (ElementNotFoundException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (BadHttpRequestException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteByIdAsync(Guid id)
    {
        try
        {
            await _service.DeleteByIdAsync(id);

            return StatusCode(StatusCodes.Status200OK);
        }
        catch (ElementNotFoundException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (BadHttpRequestException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Product>>> SearchProductsAsync(
        [FromQuery(Name = "name")] string? title = null, [FromQuery(Name = "category")] string? category = null)
    {
        try
        {
            IEnumerable<Product> products = new List<Product>();

            if (!string.IsNullOrEmpty(title))
                products = await _service.GetByTitleAsync(title);
            else if (!string.IsNullOrEmpty(category))
                products = await _service.GetByCategoryAsync(category);

            return StatusCode(StatusCodes.Status200OK, products);
        }
        catch (BadHttpRequestException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}