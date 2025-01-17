using Microsoft.AspNetCore.Mvc;
using TeamCommunicationPlatform.Models;
using System.Linq;

namespace TeamCommunicationPlatform.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly TeamCommunicationContext _context;

        public AuthenticationController(TeamCommunicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            // Record login time
            var loginLog = new UserLoginLog
            {
                UserId = user.Id,
                LoginTime = DateTime.Now
            };
            _context.UserLoginLogs.Add(loginLog);
            _context.SaveChanges();

            // Set session data
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("Name", user.Name);

            // Redirect based on role
            return RedirectToAction("Dashboard", "Home");

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ViewBag.Error = "Email already registered.";
                return View();
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                // Find the latest login record for the user and set the logout time
                var lastLogin = _context.UserLoginLogs
                    .Where(log => log.UserId == userId.Value && log.LogoutTime == null)
                    .OrderByDescending(log => log.LoginTime)
                    .FirstOrDefault();

                if (lastLogin != null)
                {
                    lastLogin.LogoutTime = DateTime.Now;
                    _context.SaveChanges();
                }
            }

            // Clear session
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
