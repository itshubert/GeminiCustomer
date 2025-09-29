using ErrorOr;
using GeminiCatalog.Domain.Common.DomainErrors;
using GeminiCustomer.Application.Common.Interfaces;
using GeminiCustomer.Application.Common.Models.Customers;
using GeminiCustomer.Domain.Customers;
using MapsterMapper;
using MediatR;

namespace GeminiCustomer.Application.Customers.Commands;

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string Email) : IRequest<ErrorOr<CustomerModel>>;

public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<CustomerModel>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<CustomerModel>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var existingCustomer = _customerRepository.GetByEmailAsync(request.Email, cancellationToken).Result;

        if (existingCustomer is not null)
        {
            return Errors.Customer.EmailAlreadyExists;
        }

        var customer = Customer.Create(
            null,
            request.FirstName,
            request.LastName,
            request.Email,
            DateTimeOffset.UtcNow,
            null);

        if (customer.IsError)
        {
            return customer.Errors;
        }

        await _customerRepository.AddAsync(customer.Value, cancellationToken);

        return _mapper.Map<CustomerModel>(customer.Value);
    }
}