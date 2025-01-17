using Microsoft.AspNetCore.Mvc;

namespace TeamCommunicationPlatform.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            // Admin-specific logic can go here, like managing users, viewing logs, etc.
            return View();
        }
    }
}
