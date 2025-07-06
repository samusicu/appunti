using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCodeFirst.Models
{
    public class TestCodeFirstContext : DbContext
    {
        public TestCodeFirstContext(DbContextOptions<TestCodeFirstContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
