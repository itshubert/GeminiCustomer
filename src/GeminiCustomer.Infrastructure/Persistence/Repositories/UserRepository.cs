using GeminiCustomer.Application.Common.Interfaces;
using GeminiCustomer.Domain.Customers.ValueObjects;
using GeminiCustomer.Domain.Users;
using GeminiCustomer.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GeminiCustomer.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(GeminiCustomerDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userId = UserId.Create(id);
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetByIdWithCustomerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userId = UserId.Create(id);
        return await _context.Users
            .Include(u => u.Customer)
                .ThenInclude(c => c.Addresses) // Include customer addresses as well
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User?> GetByUsernameWithCustomerAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.Customer)
                .ThenInclude(c => c.Addresses) // Include customer addresses as well
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var customerIdValue = CustomerId.Create(customerId);
        return await _context.Users
            .Where(u => u.CustomerId == customerIdValue)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}