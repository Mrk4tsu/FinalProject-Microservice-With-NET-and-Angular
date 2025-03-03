using FN.DataAccess.Entities;
using FN.DataAccess.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FN.DataAccess.Configurations
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.ToTable("product_details");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.ItemId).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.Detail).IsRequired();
            builder.Property(x => x.LikeCount).HasDefaultValue(0).IsRequired();
            builder.Property(x => x.DislikeCount).HasDefaultValue(0).IsRequired();
            builder.Property(x => x.DownloadCount).HasDefaultValue(0).IsRequired();
            builder.Property(x => x.Version).HasMaxLength(10);
            builder.Property(x => x.Note).HasMaxLength(250);
            builder.Property(x => x.Status).HasDefaultValue(ProductType.PUBLIC).IsRequired();

            builder.HasIndex(x => x.ItemId).HasDatabaseName("idx_productDetail_itemId");
            builder.HasIndex(x => x.CategoryId).HasDatabaseName("idx_productDetail_categoryId");
            builder.HasOne(x => x.Item).WithMany(x => x.ProductDetails).HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Category).WithMany(x => x.ProductDetails).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
