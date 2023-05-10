using Microsoft.EntityFrameworkCore;

namespace LoyaltySystem.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Member> Member { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Additional database configurations, if needed
        }
    }
}
