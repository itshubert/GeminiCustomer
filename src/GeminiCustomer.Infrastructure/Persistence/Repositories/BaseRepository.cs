namespace GeminiCustomer.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository
{
    protected readonly GeminiCustomerDbContext _context;

    protected BaseRepository(GeminiCustomerDbContext context)
    {
        _context = context;
    }
}