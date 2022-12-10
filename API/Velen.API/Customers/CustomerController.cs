using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velen.Application.Customers.GetCustomerDetails;
using Velen.Application.Customers.RegisterCustomer;
using Velen.Domain.Customers;
using CustomerRegisteredNotification = Velen.Application.Customers.IntegrationHandlers.CustomerRegisteredNotification;

namespace Velen.API.Customers;

[ApiController]
[Route("[controller]")]
public class CustomerController : AppController
{
    private IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerDetailsQuery(Guid id)
    {
        var result = await _mediator.Send(new GetCustomerDetailsQuery(id));
        return Success(result);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerRequest request)
    {
        var result = await _mediator.Send(new RegisterCustomerCommand(request.Email, request.Name));
        return Success(result);
    }
}