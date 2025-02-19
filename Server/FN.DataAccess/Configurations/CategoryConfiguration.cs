using FN.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FN.DataAccess.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Other).HasMaxLength(50);
            builder.Property(x => x.SeoAlias).IsRequired().HasMaxLength(50);
            builder.Property(x => x.SeoDescription).HasMaxLength(150);
            builder.Property(x => x.SeoImage).HasMaxLength(250);
            builder.Property(x => x.SeoKeyword).HasMaxLength(70);

            builder.HasIndex(x => x.SeoAlias).HasDatabaseName("idx_seo_alias").IsUnique();
        }
    }
}
