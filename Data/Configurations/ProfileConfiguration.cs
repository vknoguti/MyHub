using MyHub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyHub.Data.Configurations
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>, IEntityDataBaseConfiguration
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasDefaultValueSql("NEWID()");
        }
    }
}
