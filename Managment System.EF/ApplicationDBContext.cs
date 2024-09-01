using Managment_System.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Managment_System.EF
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TaskModel> TaskModels { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<Reminder> Reminders { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

       
    }
}
