using Microsoft.AspNetCore.Mvc;

namespace Virgo.Presentation.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
