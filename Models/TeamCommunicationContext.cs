using Microsoft.EntityFrameworkCore;
namespace TeamCommunicationPlatform.Models
{
    public class TeamCommunicationContext : DbContext
    {
        public TeamCommunicationContext(DbContextOptions<TeamCommunicationContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserLoginLog> UserLoginLogs { get; set; }

    }
}
