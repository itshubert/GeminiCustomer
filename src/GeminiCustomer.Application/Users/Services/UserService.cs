using GeminiCustomer.Application.Common.Interfaces;
using GeminiCustomer.Application.Common.Interfaces.Services;
using GeminiCustomer.Domain.Customers.ValueObjects;
using GeminiCustomer.Domain.Users;

namespace GeminiCustomer.Application.Users.Services;

/// <summary>
/// Example service demonstrating how to work with the User-Customer relationship
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;

    public UserService(IUserRepository userRepository, ICustomerRepository customerRepository)
    {
        _userRepository = userRepository;
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Creates a new user for an existing customer
    /// </summary>
    public async Task<User> CreateUserForCustomerAsync(
        Guid customerId,
        string username,
        string passwordHash,
        string passwordSalt,
        CancellationToken cancellationToken = default)
    {
        // Verify customer exists
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {customerId} not found.");
        }

        // Create user with CustomerId value object
        var user = User.Create(
            id: null,
            customerId: CustomerId.Create(customerId),
            username: username,
            passwordHash: passwordHash,
            passwordSalt: passwordSalt,
            isActive: true,
            token: null,
            refreshToken: null,
            tokenExpiry: null);

        await _userRepository.AddAsync(user, cancellationToken);
        return user;
    }

    /// <summary>
    /// Gets a user with their associated customer information
    /// </summary>
    public async Task<User?> GetUserWithCustomerAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // This will include the Customer and Customer.Addresses navigation properties
        return await _userRepository.GetByIdWithCustomerAsync(userId, cancellationToken);
    }

    /// <summary>
    /// Gets a user by username with their customer information
    /// </summary>
    public async Task<User?> GetUserByUsernameWithCustomerAsync(string username, CancellationToken cancellationToken = default)
    {
        // This will include the Customer and Customer.Addresses navigation properties
        return await _userRepository.GetByUsernameWithCustomerAsync(username, cancellationToken);
    }

    /// <summary>
    /// Gets all users for a specific customer
    /// </summary>
    public async Task<IEnumerable<User>> GetUsersForCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetByCustomerIdAsync(customerId, cancellationToken);
    }

    public async Task<User?> GetUserByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetByCustomerIdAsync(customerId, cancellationToken);
        return users.FirstOrDefault();
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}