using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var adminId = "1";
            var userId = "2";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = userId,
                    ConcurrencyStamp = userId,
                    Name = "user".ToUpper(),
                    NormalizedName = "user"
                },
                new IdentityRole
                {
                    Id = adminId,
                    ConcurrencyStamp = adminId,
                    Name = "admin".ToUpper(),
                    NormalizedName = "admin"
                }
            };
            
            builder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
