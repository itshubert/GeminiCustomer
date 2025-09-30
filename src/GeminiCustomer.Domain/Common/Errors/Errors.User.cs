using ErrorOr;

namespace GeminiCatalog.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class User
    {
        public static Error CustomerAlreadyHasUser => Error.Conflict(
            code: "Customer.CustomerAlreadyHasUser",
            description: "A customer with the provided user already has a User attached to it.");
    }
}