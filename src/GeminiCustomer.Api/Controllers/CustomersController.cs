using GeminiCustomer.Contracts;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiCustomer.Api.Controllers;

[Route("[controller]")]
public sealed class CustomersController : ApiController
{
    public CustomersController(ISender mediator, IMapper mapper) : base(mediator, mapper)
    {
    }

    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetCustomerById(Guid customerId)
    {
        var query = new Application.Customers.Queries.GetCustomerByIdQuery(customerId);
        var result = await Mediator.Send(query);

        return result.Match(
            customer => Ok(Mapper.Map<CustomerResponse>(customer)),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        var command = Mapper.Map<Application.Customers.Commands.CreateCustomerCommand>(request);
        var result = await Mediator.Send(command);

        return result.Match(
            customer =>
            {
                var customerResponse = Mapper.Map<CustomerResponse>(customer);
                return CreatedAtAction(
                    nameof(GetCustomerById),
                    new { customerId = customerResponse.Id },
                    customerResponse);
            },
            Problem);
    }
}