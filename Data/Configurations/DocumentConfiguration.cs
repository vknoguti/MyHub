using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHub.Entities;

namespace MyHub.Data.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document<Guid>>, IEntityDataBaseConfiguration
    {
        public void Configure(EntityTypeBuilder<Document<Guid>> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.User)
                .WithMany(t => t.Documents)
                .HasForeignKey(t => t.UserId);
        }
    }
}
