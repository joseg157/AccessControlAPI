using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This ensures that the database generates a new GUID for the Id when a new user is added.

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}
