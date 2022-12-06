using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velen.Application.Customers;

namespace Velen.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Hello([FromServices]IMediator mediator)
    {
        var result=mediator.Send(new RegisterCustomerCommand(null,"jon"));
        return Ok("Hello World");
    }
}