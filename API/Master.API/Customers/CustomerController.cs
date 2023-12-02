using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Master.Application.Customers.GetCustomerDetails;
using Master.Application.Customers.RegisterCustomer;
using Master.Infrastructure.Localization;

namespace Master.API.Customers;

/// <summary>
/// CustomerController
/// </summary>
[ApiController]
[Route("[controller]")]
public class CustomerController : AppController
{
    private readonly IMediator _mediator;

    private readonly IStringLocalizer<MultiLanguage> _stringLocalize;
    /// <summary>
    /// CustomerController
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="stringLocalize"></param>
    public CustomerController(IMediator mediator, IStringLocalizer<MultiLanguage> stringLocalize)
    {
        _mediator = mediator;
        _stringLocalize = stringLocalize;
    }

    /// <summary>
    /// GetCustomerDetailsQuery
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerDetailsQuery(Guid id)
    {
        var result = await _mediator.Send(new GetCustomerDetailsQuery(id));
        return Success(result);
    }

    /// <summary>
    /// RegisterCustomer
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerRequest request)
    {
        var result = await _mediator.Send(new RegisterCustomerCommand(request.Email, request.Name));
        return Success(result, _stringLocalize["operation_success"]);
    }
}