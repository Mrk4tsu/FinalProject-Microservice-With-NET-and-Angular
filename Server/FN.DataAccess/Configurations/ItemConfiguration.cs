using FN.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FN.DataAccess.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("items");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).HasMaxLength(150).IsRequired().IsUnicode();
            builder.Property(x => x.Code).HasMaxLength(10).IsRequired();
            builder.Property(x => x.NormalizeName).HasMaxLength(150).IsRequired();
            builder.Property(x => x.SeoTitle).HasMaxLength(150).IsRequired();
            builder.Property(x => x.SeoAlias).HasMaxLength(150).IsRequired();
            builder.Property(x => x.SeoDescription).HasMaxLength(150).IsRequired();
            builder.Property(x => x.SeoKeyword).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Thumbnail).HasMaxLength(150).IsRequired();
            builder.Property(x => x.ViewCount).HasDefaultValue(0);
            builder.Property(x => x.CreatedDate).HasDefaultValue(DateTime.UtcNow);
            builder.Property(x => x.ModifiedDate).HasDefaultValue(DateTime.UtcNow);

            builder.HasOne(x => x.Category).WithMany(x => x.Items).HasForeignKey(x => x.CategoryId);
            builder.HasOne(x => x.User).WithMany(x => x.Items).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.ProductDetails).WithOne(x => x.Item).HasForeignKey(x => x.ItemId);

            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasIndex(x => x.SeoAlias).IsUnique();
        }
    }
}
