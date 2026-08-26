using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHub.Entities;

namespace MyHub.Data.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>, IEntityDataBaseConfiguration
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Profile)
                .WithMany(t => t.Documents)
                .HasForeignKey(t => t.ProfileId);

            builder.Property(t => t.UploadedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }
    }
}
