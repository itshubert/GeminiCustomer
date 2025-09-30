using GeminiCustomer.Application.Customers.Commands;
using GeminiCustomer.Application.Users.Commands;
using GeminiCustomer.Contracts;
using GeminiCustomer.Domain.Users;
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
        var command = Mapper.Map<CreateCustomerCommand>(request);
        var customerResult = await Mediator.Send(command);
        CreateCustomerResponse createResponse;

        if (!customerResult.IsError && !request.CreateUser)
        {
            return customerResult.Match(
                customer =>
                {
                    var customerResponse = Mapper.Map<CustomerResponse>(customer);
                    createResponse = new CreateCustomerResponse(customerResponse, null);

                    return CreatedAtAction(
                        nameof(GetCustomerById),
                        new { customerId = customerResponse.Id },
                        createResponse);
                },
                Problem);
        }

        var createUserCommand = new CreateUserCommand(
            customerResult.Value.Id,
            request.Email,
            request.Password ?? "DefaultPassword123!");

        var userResult = await Mediator.Send(createUserCommand);

        return userResult.Match(
            user =>
            {
                var userResponse = Mapper.Map<UserResponse>(user);
                var customerResponse = Mapper.Map<CustomerResponse>(customerResult.Value);
                createResponse = new CreateCustomerResponse(
                    customerResponse,
                    userResponse);

                return CreatedAtAction(
                    nameof(GetCustomerById),
                    new { customerId = customerResponse.Id },
                    createResponse);
            },
            Problem);

    }

    [HttpPost("{customerId:guid}/addresses")]
    public async Task<IActionResult> AddAddress(Guid customerId, [FromBody] CreateAddressRequest request)
    {
        var command = Mapper.Map<AddAddressCommand>(request) with { CustomerId = customerId };
        var result = await Mediator.Send(command);

        return result.Match(
            address => Ok(Mapper.Map<AddressResponse>(address)),
            Problem);
    }
}