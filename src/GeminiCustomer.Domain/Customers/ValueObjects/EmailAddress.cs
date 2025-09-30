using ErrorOr;
using GeminiCustomer.Domain.Common.Models;

namespace GeminiCustomer.Domain.Customers.ValueObjects;

public sealed class EmailAddress : ValueObject
{
    public string Value { get; private set; }

    private EmailAddress(string value)
    {
        Value = value;
    }

    public static ErrorOr<EmailAddress> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Error.Validation(
                code: "Customer.Email.Empty",
                description: "Email must not be empty.");
        }

        if (email.Length > CustomerValidationConstants.MaxEmailLength)
        {
            return Error.Validation(
                code: "Customer.Email.TooLong",
                description: $"Email must not exceed {CustomerValidationConstants.MaxEmailLength} characters.");
        }

        if (!IsValidEmailFormat(email))
        {
            return Error.Validation(
                code: "Customer.Email.Invalid",
                description: "Email format is invalid.");
        }

        return new EmailAddress(email.Trim().ToLowerInvariant());
    }

    private static bool IsValidEmailFormat(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(EmailAddress emailAddress) => emailAddress.Value;
}