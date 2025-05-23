using Microsoft.EntityFrameworkCore;

namespace SPInRepositoryPro.Models
{
    public class ProgramDbContext : DbContext
    {

        public ProgramDbContext() : base()
        {

        }
        public ProgramDbContext(DbContextOptions<ProgramDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=LAPTOP-46S21PDI;Database=SpInRepositoryPro;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

    }
}
