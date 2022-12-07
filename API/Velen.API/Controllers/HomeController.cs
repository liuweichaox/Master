using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velen.Application.Customers;

namespace Velen.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : AppController
{
    [HttpGet]
    public async Task<IActionResult> Hello([FromServices] IMediator mediator)
    {
        var result =await mediator.Send(new RegisterCustomerCommand("893703953@qq.com", "liu wei chao"));
        return Success(result);
    }
}