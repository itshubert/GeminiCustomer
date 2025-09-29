using ErrorOr;

namespace GeminiCatalog.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class Customer
    {
        public static Error NotFound => Error.NotFound(
            code: "Customer.NotFound",
            description: "The specified customer was not found.");

        public static Error InvalidCustomerId => Error.Validation(
            code: "Customer.InvalidId",
            description: "The provided customer ID is invalid.");
    }
}