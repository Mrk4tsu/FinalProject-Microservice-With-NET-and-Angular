using FN.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FN.DataAccess.Configurations
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("app_roles");
            builder.Property(x => x.Description)
                .HasMaxLength(100)
                .IsRequired()
                .IsUnicode();

            builder.HasIndex(x => x.NormalizedName)
                .HasDatabaseName("idx_app_roles_normalized_name");
        }
    }
}
