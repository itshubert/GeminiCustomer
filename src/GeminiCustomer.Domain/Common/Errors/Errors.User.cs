using ErrorOr;

namespace GeminiCatalog.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error CustomerAlreadyHasUser => Error.Conflict(
            code: "Customer.CustomerAlreadyHasUser",
            description: "A customer with the provided user already has a User attached to it.");

        public static Error InvalidCredentials => Error.Validation(
            code: "User.InvalidCredentials",
            description: "The provided credentials are invalid.");

        public static Error UserIsNotActive => Error.Validation(
            code: "User.UserIsNotActive",
            description: "The user is not active.");
    }
}