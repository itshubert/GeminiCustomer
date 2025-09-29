using ErrorOr;
using GeminiCatalog.Domain.Common.DomainErrors;
using GeminiCustomer.Application.Common.Interfaces;
using GeminiCustomer.Application.Common.Models.Addresses;
using MapsterMapper;
using MediatR;

namespace GeminiCustomer.Application.Customers.Commands;

public sealed record AddAddressCommand(
    Guid CustomerId,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostCode,
    string Country,
    bool IsDefault) : IRequest<ErrorOr<AddressModel>>;

public sealed class AddAddressCommandHandler(
    ICustomerRepository customerRepository,
    IMapper mapper) : IRequestHandler<AddAddressCommand, ErrorOr<AddressModel>>
{
    public async Task<ErrorOr<AddressModel>> Handle(
        AddAddressCommand command,
        CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
        if (customer is null)
        {
            return Errors.Customer.InvalidCustomerId;
        }

        var addressResult = customer.CreateAddress(
            addressLine1: command.AddressLine1,
            addressLine2: command.AddressLine2,
            city: command.City,
            state: command.State,
            postCode: command.PostCode,
            country: command.Country,
            isDefault: command.IsDefault);

        if (addressResult.IsError)
        {
            return addressResult.Errors;
        }

        await customerRepository.UpdateAsync(customer, cancellationToken);

        return mapper.Map<AddressModel>(addressResult.Value);
    }
}