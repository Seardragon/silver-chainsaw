using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Data
{
    public class CompensationContext : DbContext
    {
        public CompensationContext(DbContextOptions<CompensationContext> options) : base(options)
        {

        }

        public DbSet<Compensation> Compensations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignore Employee since it's not in the same DB, we add the proper employee later by employeeId fetch
            modelBuilder.Entity<Compensation>()
                .Ignore(c => c.Employee)
                .HasKey(c => c.EmployeeId);       
        }
    }
}
