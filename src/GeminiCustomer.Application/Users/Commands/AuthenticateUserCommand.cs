using ErrorOr;
using GeminiCatalog.Domain.Common.DomainErrors;
using GeminiCustomer.Application.Common.Interfaces.Authentication;
using GeminiCustomer.Application.Common.Interfaces.Services;
using GeminiCustomer.Application.Common.Models.Customers;
using GeminiCustomer.Application.Common.Models.Users;
using MapsterMapper;
using MediatR;

namespace GeminiCustomer.Application.Users.Commands;

public sealed record AuthenticateUserCommand(string Username, string Password) : IRequest<ErrorOr<AuthenticationResultModel>>;

public sealed class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, ErrorOr<AuthenticationResultModel>>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticateUserCommandHandler(
        IUserService userService,
        IMapper mapper,
        IJwtTokenGenerator tokenGenerator,
        IPasswordHasher passwordHasher)
    {
        _userService = userService;
        _mapper = mapper;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<AuthenticationResultModel>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByUsernameWithCustomerAsync(request.Username, cancellationToken);
        if (user is null)
        {
            return Errors.User.InvalidCredentials;
        }

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash, Convert.FromHexString(user.PasswordSalt)))
        {
            return Errors.User.InvalidCredentials;
        }

        if (!user.IsActive)
        {
            return Errors.User.UserIsNotActive;
        }

        var token = _tokenGenerator.GenerateToken(user);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();
        var tokenExpiry = DateTimeOffset.UtcNow.AddMinutes(15);

        user.UpdateToken(
            token.TokenString,
            refreshToken.TokenString,
            tokenExpiry);

        await _userService.UpdateUserAsync(user, cancellationToken);

        return new AuthenticationResultModel(
            new UserModel(
                user.Id.Value,
                user.CustomerId.Value,
                user.Username,
                user.IsActive,
                token.TokenString,
                refreshToken.TokenString,
                tokenExpiry),
            _mapper.Map<CustomerModel>(user.Customer),
            token.TokenString,
            refreshToken.TokenString,
            tokenExpiry);
    }
}