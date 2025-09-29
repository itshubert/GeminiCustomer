using ErrorOr;
using GeminiCatalog.Domain.Common.DomainErrors;
using GeminiCustomer.Application.Common.Interfaces;
using GeminiCustomer.Application.Common.Models.Customers;
using MapsterMapper;
using MediatR;

namespace GeminiCustomer.Application.Customers.Queries;

public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<ErrorOr<CustomerModel>>;

public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ErrorOr<CustomerModel>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<CustomerModel>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (customer is null)
        {
            return Errors.Customer.NotFound;
        }

        var customerModel = _mapper.Map<CustomerModel>(customer);
        return customerModel;
    }
}