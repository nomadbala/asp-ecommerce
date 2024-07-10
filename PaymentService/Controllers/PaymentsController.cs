using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Contracts;
using PaymentService.Models;
using PaymentService.Services;
using Exception = System.Exception;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentsService _service;

    public PaymentsController(IPaymentsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payment>>> GetAllAsync()
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
    public async Task<ActionResult<Payment>> CreateAsync([FromBody] CreatePaymentContract contract)
    {
        try
        {
            return StatusCode(StatusCodes.Status201Created, await _service.CreateAsync(contract));
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

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> GetByIdAsync(Guid id)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.GetByIdAsync(id));
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
    public async Task<ActionResult<Payment>> UpdateAsync(Guid id, [FromBody] UpdatePaymentContract contract)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.UpdateAsync(id, contract));
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
    public async Task<ActionResult<Payment>> DeleteByIdAsync(Guid id)
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
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Payment>>> SearchAsync(
        [FromQuery(Name = "user")] Guid? userId = null,
        [FromQuery(Name = "order")] Guid? orderId = null,
        [FromQuery(Name = "status")] PaymentStatus? status = null
    )
    {
        try
        {
            IEnumerable<Payment> payments;

            if (userId != null)
            {
                payments = await _service.GetByUserIdAsync(userId.Value);
            }
            else if (orderId != null)
            {
                payments = await _service.GetByOrderIdAsync(orderId.Value);
            }
            else if (status != null)
            {
                payments = await _service.GetByStatusAsync(status.Value);
            }
            else
            {
                payments = await _service.GetAllAsync();
            }

            return StatusCode(StatusCodes.Status200OK, payments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}