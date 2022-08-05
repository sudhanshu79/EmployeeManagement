using EmployeeManagement.Models.Employees;
using EmployeeManagement.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();

            foreach (var foriegnKey in modelBuilder.Model.GetEntityTypes()
                                                                         .SelectMany(x => x.GetForeignKeys()))
            {
                foriegnKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
