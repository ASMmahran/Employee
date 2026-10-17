using Microsoft.EntityFrameworkCore;
using Employee.Models;

namespace Employee.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Employeee> Employees => Set<Employeee>();
    }
}
