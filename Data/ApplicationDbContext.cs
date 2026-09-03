using MyHub.Data.Configurations;
using MyHub.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyHub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(
                assembly: typeof(ApplicationDbContext).Assembly,
                predicate: type => typeof(IEntityDataBaseConfiguration).IsAssignableFrom(type)
            );
        }
    }
}
