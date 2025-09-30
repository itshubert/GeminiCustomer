using ErrorOr;

namespace GeminiCustomer.Domain.Customers.ValueObjects;

public class ValidationBuilder
{
    private readonly List<Error> _errors = new();

    public ValidationBuilder ValidateRequired(string value, string fieldName, string displayName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _errors.Add(Error.Validation(
                code: $"Customer.{fieldName}.Empty",
                description: $"{displayName} must not be empty."));
        }
        return this;
    }

    public ValidationBuilder ValidateMaxLength(string value, string fieldName, string displayName, int maxLength)
    {
        if (!string.IsNullOrWhiteSpace(value) && value.Length > maxLength)
        {
            _errors.Add(Error.Validation(
                code: $"Customer.{fieldName}.TooLong",
                description: $"{displayName} must not exceed {maxLength} characters."));
        }
        return this;
    }

    public ValidationBuilder ValidateEmail(string email)
    {
        if (!string.IsNullOrWhiteSpace(email) && !IsValidEmailFormat(email))
        {
            _errors.Add(Error.Validation(
                code: "Customer.Email.Invalid",
                description: "Email format is invalid."));
        }
        return this;
    }

    public ValidationBuilder ValidateCustom(Func<bool> predicate, Error error)
    {
        if (!predicate())
        {
            _errors.Add(error);
        }
        return this;
    }

    public ErrorOr<Success> Build()
    {
        return _errors.Count > 0 ? _errors : Result.Success;
    }

    public bool HasErrors => _errors.Count > 0;
    public IReadOnlyList<Error> Errors => _errors.AsReadOnly();

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
}