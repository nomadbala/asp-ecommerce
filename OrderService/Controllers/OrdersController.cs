using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using OrderService.Contracts;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService _service;

    public OrdersController(IOrdersService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAllAsync()
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
    public async Task<ActionResult<Order>> CreateAsync([FromBody] CreateOrderContract contract)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);

        try
        {
            return StatusCode(StatusCodes.Status201Created, await _service.CreateAsync(contract));
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

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetByIdAsync(Guid id)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.GetByIdAsync(id));
        }
        catch (BadHttpRequestException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (ElementNotFoundException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Order>> UpdateAsync(Guid id, [FromBody] UpdateOrderContract contract)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);

        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.UpdateAsync(id, contract));
        }
        catch (BadHttpRequestException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.StatusCode);
        }
        catch (ElementNotFoundException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
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
        catch (BadHttpRequestException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (ElementNotFoundException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Order>>> SearchOrdersAsync(
        [FromQuery(Name = "user")] Guid? userId = null, [FromQuery(Name = "status")] OrderStatus? status = null)
    {
        try
        {
            if (userId != null && status != null)
                return StatusCode(StatusCodes.Status400BadRequest, "Specify either 'user' or 'status', not both.");

            if (userId != null)
                return StatusCode(StatusCodes.Status200OK, await _service.GetByCustomerIdAsync(userId.Value));

            if (status != null)
                return StatusCode(StatusCodes.Status200OK, await _service.GetByStatusAsync(status.Value));

            return StatusCode(StatusCodes.Status400BadRequest, "Specify either 'user' or 'status'.");
        }
        catch (BadHttpRequestException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}