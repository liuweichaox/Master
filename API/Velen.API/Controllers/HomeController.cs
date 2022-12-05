using Microsoft.AspNetCore.Mvc;

namespace Velen.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Hello()
    {
        return Ok("Hello World");
    }
}