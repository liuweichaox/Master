using Master.Application.Customers.GetCustomerDetails;
using Master.Application.Customers.RegisterCustomer;
using Master.Infrastructure.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Master.API.Controller;

/// <summary>
///     CustomerController
/// </summary>
[ApiController]
[Route("[controller]")]
public class CustomerController : AppController
{
    private readonly IMediator _mediator;

    private readonly IStringLocalizer<MultiLanguage> _stringLocalize;

    /// <summary>
    ///     CustomerController
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="stringLocalize"></param>
    public CustomerController(IMediator mediator, IStringLocalizer<MultiLanguage> stringLocalize)
    {
        _mediator = mediator;
        _stringLocalize = stringLocalize;
    }

    /// <summary>
    ///     GetCustomerDetailsQuery
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerDetailsQuery(Guid id)
    {
        //var result = await _mediator.Send(new GetCustomerDetailsQuery(id));
        return Success("result", _stringLocalize["operation_success"]);
    }

    /// <summary>
    ///     RegisterCustomer
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return Success(result, _stringLocalize["operation_success"]);
    }
}