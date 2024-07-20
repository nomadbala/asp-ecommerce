using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using UserService.Contracts;
using UserService.Models;

namespace UserService.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserServiceDatabaseContext _context;

    public UsersRepository(UserServiceDatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> CreateAsync(CreateUserContract contract)
    {
        if (await _context.Users.AnyAsync(u => u.Email == contract.Email))
            throw new ElementAlreadyExistsException($"User with email {contract.Email} already exists");

        var user = new User(contract);

        await _context.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        return user ?? throw new ElementNotFoundException($"User with id {id} not found");
    }

    public async Task<User> UpdateAsync(Guid id, UpdateUserContract contract)
    {
        var user = await GetByIdAsync(id);

        if (!await _context.Users.AnyAsync(u => u.Email == contract.Email))
            throw new ElementAlreadyExistsException($"User with email {contract.Email} already exists");

        user.FullName = contract.FullName;
        user.Email = contract.Email;
        user.Address = contract.Address;

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var user = await GetByIdAsync(id);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetByNameAsync(string fullName)
    {
        return await _context.Users
            .Where(u => u.FullName == fullName)
            .ToListAsync();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        return user ?? throw new ElementNotFoundException($"User with email {email} not found");   
    }
}