using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamPlanner.Models;

namespace TeamPlanner.Data
{
    public class ApplicationDbContext : IdentityDbContext<Account>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<UnavailableTime> UnavailableTimes { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
