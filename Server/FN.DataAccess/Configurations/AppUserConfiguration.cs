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
            builder.Property(x => x.Avatar)
                .HasMaxLength(255)
                .HasDefaultValue("https://res.cloudinary.com/dje3seaqj/image/upload/v1736989161/gatapchoi_biglrl.jpg");
            builder.Property(x => x.TimeCreated)
                .HasDefaultValue(DateTime.Now);

            builder.HasIndex(x => x.UserName).HasDatabaseName("idx_appUser_userName").IsUnique();
        }
    }
}
