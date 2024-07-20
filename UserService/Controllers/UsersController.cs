using System.Text.Json;
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

    private readonly ILogger<UsersController> _logger;

    public UsersController(IUsersService service, ILogger<UsersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllAsync()
    {
        try
        {
            var users = await _service.GetAllAsync();
            
            _logger.LogInformation("Users successfully received");
            
            return StatusCode(StatusCodes.Status200OK, users);
        }
        catch (Exception e)
        {
            _logger.LogError($"Internal server error: {JsonSerializer.Serialize(e.Message)}");
            
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateAsync([FromBody] CreateUserContract contract)
    {
        _logger.LogInformation($"Received CreateUserContract: {JsonSerializer.Serialize(contract)}");

        if (!ModelState.IsValid)
        {
            _logger.LogError($"ModelState is invalid: {JsonSerializer.Serialize(ModelState)}");
            
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }

        try
        {
            var user = await _service.CreateAsync(contract);
            
            _logger.LogInformation("User created successfully");
            
            return StatusCode(StatusCodes.Status201Created, user);
        }
        catch (ElementAlreadyExistsException e)
        {
            _logger.LogError($"ElementAlreadyExistsException: {e.Message}");
            
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError($"Internal server error: {e.Message}");
            
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetByIdAsync(Guid id)
    {
        _logger.LogInformation($"Received id: {id}");
        
        try
        {
            var user = await _service.GetByIdAsync(id);
            
            _logger.LogInformation($"User successfully received");
            
            return StatusCode(StatusCodes.Status200OK, user);
        }
        catch (ElementNotFoundException e)
        {
            _logger.LogError($"ElementNotFoundException: {e.Message}");   
            
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (BadHttpRequestException e)
        {
            _logger.LogError($"BadHttpRequestException: {e.Message}");
            
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError($"Internal server error: {e.Message}");
            
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> UpdateAsync(Guid id, [FromBody] UpdateUserContract contract)
    {
        _logger.LogInformation($"Received id: {id}");
        _logger.LogInformation($"Received UpdateUserContract: {JsonSerializer.Serialize(contract)}");
        
        try
        {
            var user = await _service.UpdateAsync(id, contract);
            
            _logger.LogInformation($"User updated successfully");
            
            return StatusCode(StatusCodes.Status200OK, user);
        }
        catch (ElementNotFoundException e)
        {
            _logger.LogError($"ElementNotFoundException: {e.Message}");
            
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (ElementAlreadyExistsException e)
        {
            _logger.LogError($"ElementAlreadyExistsException: {e.Message}");
            
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (BadHttpRequestException e)
        {
            _logger.LogError($"BadHttpRequestException: {e.Message}");
            
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError($"Internal server error: {e.Message}");
            
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteByIdAsync(Guid id)
    {
        _logger.LogInformation($"Received id: {id}");
        
        try
        {
            await _service.GetByIdAsync(id);
            
            _logger.LogInformation("User deleted succesfully");

            return StatusCode(StatusCodes.Status200OK);
        }
        catch (ElementNotFoundException e)
        {
            _logger.LogError($"ElementNotFoundException: {e.Message}");
            
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (BadHttpRequestException e)
        {
            _logger.LogError($"BadHttpRequestException: ${e.Message}");
            
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError($"Internal server error: {e.Message}");
            
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetByNameOrEmailAsync([FromQuery(Name = "name")] string? fullName = null, [FromQuery(Name = "email")] string? email = null)
    {
        _logger.LogInformation("Search request received with name: {Name}, email: {Email}", fullName, email);

        try
        {
            if (!string.IsNullOrEmpty(fullName) && !string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("Both name and email provided in search query");
                return StatusCode(StatusCodes.Status400BadRequest, "Specify either 'name' or 'email', not both.");
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                var users = await _service.GetByNameAsync(fullName);
                _logger.LogInformation("Users found by name: {Name}", fullName);
                return StatusCode(StatusCodes.Status200OK, users);
            }
        
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _service.GetByEmailAsync(email);
                _logger.LogInformation("User found by email: {Email}", email);
                return StatusCode(StatusCodes.Status200OK, user);
            }

            _logger.LogWarning("Neither name nor email provided in search query");
            return StatusCode(StatusCodes.Status400BadRequest, "Either 'name' or 'email' must be provided.");
        }
        catch (ElementNotFoundException e)
        {
            _logger.LogError(e, "ElementNotFoundException: {Message}", e.Message);
            return StatusCode(StatusCodes.Status404NotFound, e.Message);
        }
        catch (BadHttpRequestException e)
        {
            _logger.LogError(e, "BadHttpRequestException: {Message}", e.Message);
            return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error: {Message}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }


}