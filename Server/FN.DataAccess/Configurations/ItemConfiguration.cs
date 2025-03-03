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

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(256).IsRequired();
            builder.Property(x => x.NormalizedTitle).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(250);
            builder.Property(x => x.Keywords).HasMaxLength(150);
            builder.Property(x => x.Thumbnail).HasMaxLength(250);
            builder.Property(x => x.ViewCount).HasDefaultValue(0);
            builder.Property(x => x.SeoAlias).HasMaxLength(256);
            builder.Property(x => x.SeoTitle).HasMaxLength(256);
            builder.Property(x => x.CreatedDate).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(x => x.ModifiedDate).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            builder.HasIndex(x => x.Code).HasDatabaseName("ix_item_code").IsUnique();

            builder.HasOne(x => x.User).WithMany(x => x.Items).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.ProductDetails).WithOne(x => x.Item).HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
