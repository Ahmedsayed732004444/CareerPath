using CareerPath.Models;
using Microsoft.EntityFrameworkCore;

namespace CareerPath.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }
        public DbSet<JopApp> JopApps { get; set; }

        // public DbSet<YourEntity> YourEntities { get; set; }
    }
}
