using UserService.Contracts;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _repository;

    public UsersService(IUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<User> CreateAsync(CreateUserContract contract)
    {
        return await _repository.CreateAsync(contract);
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new BadHttpRequestException($"Invalid ID. {id}");
        
        return await _repository.GetByIdAsync(id);
    }

    public async Task<User> UpdateAsync(Guid id, UpdateUserContract contract)
    {
        if (id == Guid.Empty)
            throw new BadHttpRequestException($"Invalid ID. {id}");
        
        return await _repository.UpdateAsync(id, contract);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await _repository.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable<User>> GetByNameAsync(string fullName)
    {
        if (string.IsNullOrEmpty(fullName))
            throw new BadHttpRequestException($"Invalid name. {nameof(fullName)}");
        
        return await _repository.GetByNameAsync(fullName);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new BadHttpRequestException($"Invalid email. {nameof(email)}");
        
        return await _repository.GetByEmailAsync(email);
    }
}