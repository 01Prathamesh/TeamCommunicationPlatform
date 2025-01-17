using Microsoft.AspNetCore.Mvc;
using TeamCommunicationPlatform.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace TeamCommunicationPlatform.Controllers
{
    public class MessageController : Controller
    {
        private readonly TeamCommunicationContext _context;

        public MessageController(TeamCommunicationContext context)
        {
            _context = context;
        }

        // Display Inbox
        [HttpGet]
        public IActionResult Inbox()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Authentication");
            }

            // Fetch messages where the current user is the receiver
            var messages = _context.Messages
                                   .Where(m => m.ReceiverId == userId.Value)
                                   .OrderByDescending(m => m.Timestamp)
                                   .ToList();

            return View(messages);
        }

        // Render Send Message Page
        [HttpGet]
        public IActionResult Send()
        {
            ViewBag.Users = _context.Users.ToList(); // Fetch all users for dropdown
            return View();
        }

        // Handle Sending of Messages
        [HttpPost]
        public IActionResult Send(int receiverId, string content)
        {
            int? senderId = HttpContext.Session.GetInt32("UserId");

            if (!senderId.HasValue)
            {
                return RedirectToAction("Login", "Authentication");
            }

            // Create and save a new message
            var message = new Message
            {
                SenderId = senderId.Value,
                ReceiverId = receiverId,
                Content = content,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Inbox");
        }
    }
}
