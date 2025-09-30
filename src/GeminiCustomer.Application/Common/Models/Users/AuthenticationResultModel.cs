using GeminiCustomer.Application.Common.Models.Customers;

namespace GeminiCustomer.Application.Common.Models.Users;

public record AuthenticationResultModel(
    UserModel User,
    CustomerModel Customer,
    string Token,
    string RefreshToken,
    DateTimeOffset TokenExpiry);