using CareerPath.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareerPath.Data
{
    public class MyContext : IdentityDbContext<UserApp>
    {

        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }
        public DbSet<JopApp> JopApps { get; set; }


        // public DbSet<YourEntity> YourEntities { get; set; }
    }
}
