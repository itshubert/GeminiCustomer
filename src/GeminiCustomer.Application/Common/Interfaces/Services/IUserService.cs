using GeminiCustomer.Domain.Users;

namespace GeminiCustomer.Application.Common.Interfaces.Services;

public interface IUserService
{
    Task<User> CreateUserForCustomerAsync(Guid customerId, string username, string passwordHash, string passwordSalt, CancellationToken cancellationToken = default);
    Task<User?> GetUserByUsernameWithCustomerAsync(string username, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersForCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<User?> GetUserWithCustomerAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> GetUserByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
}