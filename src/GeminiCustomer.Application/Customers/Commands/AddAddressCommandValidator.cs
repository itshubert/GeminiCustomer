using FluentValidation;

namespace GeminiCustomer.Application.Customers.Commands;

public sealed class AddAddressCommandValidator : AbstractValidator<AddAddressCommand>
{
    public AddAddressCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.")
            .NotEqual(Guid.Empty).WithMessage("CustomerId cannot be an empty GUID.");

        RuleFor(x => x.AddressLine1)
            .NotEmpty().WithMessage("AddressLine1 is required.")
            .MaximumLength(100).WithMessage("AddressLine1 must not exceed 100 characters.");

        RuleFor(x => x.AddressLine2)
            .MaximumLength(100).WithMessage("AddressLine2 must not exceed 100 characters.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(50).WithMessage("City must not exceed 50 characters.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required.")
            .MaximumLength(50).WithMessage("State must not exceed 50 characters.");

        RuleFor(x => x.PostCode)
            .NotEmpty().WithMessage("PostCode is required.")
            .MaximumLength(20).WithMessage("PostCode must not exceed 20 characters.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");
    }
}