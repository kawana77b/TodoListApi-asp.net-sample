using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models.Auth;
using TodoApi.Models.TODO;

namespace TodoApi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>()
               .HasIndex(x => x.Email)
               .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}