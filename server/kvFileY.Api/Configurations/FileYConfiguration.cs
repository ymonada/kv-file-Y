using kvFileY.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kvFileY.Infrastructure.Configurations;

public class FileYConfiguration : IEntityTypeConfiguration<FileY>
{
    public void Configure(EntityTypeBuilder<FileY> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.ContentType).IsRequired();
        builder.Property(x => x.FileName);
        builder.Property(x => x.FilePath);
        builder.Property(x=>x.UploadedAt);
        builder.Property(x => x.FileSize);
        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

    }
}