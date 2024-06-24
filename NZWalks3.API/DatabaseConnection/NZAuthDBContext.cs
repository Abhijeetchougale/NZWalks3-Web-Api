using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks3.API.DatabaseConnection
{
    public class NZAuthDBContext : IdentityDbContext
    {
        public NZAuthDBContext(DbContextOptions<NZAuthDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "bc789bb4-17c4-4720-8a0e-9b53990a953b";
            var writerRoleId = "78963e7a-e612-4b4c-b579-222c70f3a583";

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id =readerRoleId,
                    ConcurrencyStamp = readerRoleId ,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                },

                new IdentityRole()
                {
                    Id=writerRoleId,
                    ConcurrencyStamp=writerRoleId ,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
