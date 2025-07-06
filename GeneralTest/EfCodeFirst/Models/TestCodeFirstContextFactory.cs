using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EfCodeFirst.Models
{
    public class TestCodeFirstContextFactory : IDesignTimeDbContextFactory<TestCodeFirstContext>
    {
        public TestCodeFirstContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestCodeFirstContext>();

            // Replace with your actual connection string
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=TestCodeFirst;Trusted_Connection=True;TrustServerCertificate=True;");

            return new TestCodeFirstContext(optionsBuilder.Options);
        }
    }
}
