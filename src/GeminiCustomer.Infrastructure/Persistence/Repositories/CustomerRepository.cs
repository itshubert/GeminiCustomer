using GeminiCustomer.Application.Common.Interfaces;
using GeminiCustomer.Domain.Customers;
using GeminiCustomer.Domain.Customers.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GeminiCustomer.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository : BaseRepository, ICustomerRepository
{
    public CustomerRepository(GeminiCustomerDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customerId = CustomerId.Create(id);
        return await _context.Customers
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);

    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);
    }
}