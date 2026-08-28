
using MyHub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyHub.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>, IEntityDataBaseConfiguration
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasIndex(t => t.UserName).IsUnique();
            builder.HasIndex(t => t.Email).IsUnique();

            //builder.Property(t => t.Id).HasDefaultValueSql("uuidv7()");
            builder.Property(t => t.Id).HasDefaultValueSql("NEWID()");

            builder.HasOne(t => t.Profile)
                .WithOne(t => t.User)
                .HasForeignKey<Profile>(t => t.UserId);
        }
    }
}
