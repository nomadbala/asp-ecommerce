using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using UserService.Contracts;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _service;

    public UsersController(IUsersService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllAsync()
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
    public async Task<ActionResult<User>> CreateAsync([FromBody] CreateUserContract contract)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);

        try
        {
            return StatusCode(StatusCodes.Status201Created, await _service.CreateAsync(contract));
        }
        catch (ElementAlreadyExistsException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetByIdAsync(Guid id)
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
    public async Task<ActionResult<User>> UpdateAsync(Guid id, [FromBody] UpdateUserContract contract)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _service.UpdateAsync(id, contract));
        }
        catch (ElementNotFoundException e)
        {
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (ElementAlreadyExistsException e)
        {
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
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
            await _service.GetByIdAsync(id);

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
    public async Task<IActionResult> GetByNameOrEmailAsync([FromQuery(Name = "name")] string? fullName = null, [FromQuery(Name = "email")] string? email = null)
    {
        try
        {
            if (!string.IsNullOrEmpty(fullName) && !string.IsNullOrEmpty(email))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Specify either 'name' or 'email', not both.");
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                var users = await _service.GetByNameAsync(fullName);
                return StatusCode(StatusCodes.Status200OK, users);
            }
        
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _service.GetByEmailAsync(email);
                return StatusCode(StatusCodes.Status200OK, user);
            }

            return StatusCode(StatusCodes.Status400BadRequest, "Either 'name' or 'email' must be provided.");
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

}