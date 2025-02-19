﻿namespace TeamCommunicationPlatform.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
    }
}
