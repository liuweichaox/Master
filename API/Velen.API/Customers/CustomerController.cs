using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Velen.Application.Customers.GetCustomerDetails;
using Velen.Application.Customers.RegisterCustomer;
using Velen.Infrastructure.Localization;

namespace Velen.API.Customers;

[ApiController]
[Route("[controller]")]
public class CustomerController : AppController
{
    private IMediator _mediator;

    private IStringLocalizer<MultiLanguage> _stringLocalizer;
    public CustomerController(IMediator mediator,IStringLocalizer<MultiLanguage> stringLocalizer)
    {
        _mediator = mediator;
        _stringLocalizer = stringLocalizer;
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
        return Success(result,_stringLocalizer["operation_success"]);
    }
}