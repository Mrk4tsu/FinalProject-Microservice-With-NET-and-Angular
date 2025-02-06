using FN.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FN.DataAccess.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("app_users");

            builder.Property(x => x.FullName)
                .HasMaxLength(100)
                .IsRequired()
                .IsUnicode();

            builder.HasIndex(x => x.UserName)
                .HasDatabaseName("idx_app_username")
                .IsUnique();
        }
    }
}
