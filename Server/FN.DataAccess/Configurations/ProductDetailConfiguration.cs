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
            builder.ToTable("products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Detail).IsRequired();
            builder.Property(x => x.LikeCount).HasDefaultValue(0);
            builder.Property(x => x.DisLikeCount).HasDefaultValue(0);
            builder.Property(x => x.Version).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Note).HasMaxLength(250).IsRequired();
            builder.Property(x => x.ProductType).HasDefaultValue(ProductType.PUBLIC).IsRequired();
            builder.Property(x => x.Status).HasDefaultValue(true).IsRequired();
            builder.Property(x => x.DownloadCount).HasDefaultValue(0);

            builder.HasIndex(x => x.ItemId);
            builder.HasOne(x => x.Item).WithMany(x => x.ProductDetails).HasForeignKey(x => x.ItemId);
            builder.HasMany(x => x.ProductImages).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
            builder.HasMany(x => x.ProductPrices).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
        }
    }
}
