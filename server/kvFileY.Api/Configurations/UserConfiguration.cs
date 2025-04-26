using kvFileY.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace kvFileY.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
        builder.Property(x=>x.UserName).HasMaxLength(127).IsRequired();
        builder.Property(x => x.PasswordHash);
        
        builder
            .HasMany(x => x.Files)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}