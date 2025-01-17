using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TeamCommunicationPlatform.Models;
using System.Linq;

namespace TeamCommunicationPlatform.Controllers
{
    public class DashboardController : Controller
    {
        private readonly TeamCommunicationContext _context;

        public DashboardController(TeamCommunicationContext context)
        {
            _context = context;
        }

        // Display Dashboard
        [HttpGet]
        [HttpGet]
        [HttpGet]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Authentication");
            }

            // Fetch data for the view
            var messages = _context.Messages
                                   .Where(m => m.ReceiverId == userId.Value)
                                   .OrderByDescending(m => m.Timestamp)
                                   .ToList();

            var logs = _context.UserLoginLogs
                               .OrderByDescending(log => log.LoginTime)
                               .ToList();

            var users = _context.Users.Where(u => u.Id != userId.Value).ToList(); // Exclude current user

            ViewBag.Messages = messages;
            ViewBag.Logs = logs;
            ViewBag.Users = users;

            return View("~/Views/Home/Dashboard.cshtml"); // Explicitly specify the view path
        }



        // Send a Message
        [HttpPost]
        public IActionResult SendMessage(int receiverId, string content)
        {
            int? senderId = HttpContext.Session.GetInt32("UserId");
            if (!senderId.HasValue)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var message = new Message
            {
                SenderId = senderId.Value,
                ReceiverId = receiverId,
                Content = content,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
