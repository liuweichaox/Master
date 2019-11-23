using Microsoft.AspNetCore.Mvc;

namespace Virgo.Web.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
