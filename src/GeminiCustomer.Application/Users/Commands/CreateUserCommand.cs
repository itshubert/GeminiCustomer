using ErrorOr;
using GeminiCatalog.Domain.Common.DomainErrors;
using GeminiCustomer.Application.Common.Interfaces;
using GeminiCustomer.Application.Common.Interfaces.Authentication;
using GeminiCustomer.Application.Common.Interfaces.Services;
using GeminiCustomer.Application.Common.Models.Users;
using MediatR;

namespace GeminiCustomer.Application.Users.Commands;

public sealed record CreateUserCommand(
    Guid CustomerId,
    string Username,
    string Password) : IRequest<ErrorOr<UserModel>>;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<UserModel>>
{
    private readonly IUserService _userService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IUserService userService,
        ICustomerRepository customerRepository,
        IPasswordHasher passwordHasher)
    {
        _userService = userService;
        _customerRepository = customerRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<UserModel>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // check to ensure the customer exists and that there's no attached user
        var existingUser = await _userService.GetUserByCustomerIdAsync(command.CustomerId, cancellationToken);

        if (existingUser is not null)
        {
            return Errors.User.CustomerAlreadyHasUser;
        }

        var customer = await _customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Errors.Customer.InvalidCustomerId;
        }

        // In a real application, you would hash the password and generate a salt
        var passwordHash = _passwordHasher.HashPasword(command.Password, out var salt);


        var user = await _userService.CreateUserForCustomerAsync(
            command.CustomerId,
            command.Username,
            passwordHash,
            Convert.ToHexString(salt),
            cancellationToken);

        var userModel = new UserModel(
            user.Id.Value,
            user.CustomerId.Value,
            user.Username,
            user.IsActive,
            user.Token,
            user.RefreshToken,
            user.TokenExpiry);

        return userModel;
    }
}