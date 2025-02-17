using FN.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FN.DataAccess.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("product_images");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.UrlImage).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Alt).HasMaxLength(150).IsRequired();
            builder.Property(x => x.PublicId).HasMaxLength(50).IsRequired();

            builder.HasIndex(x => x.PublicId).IsUnique();
            builder.HasIndex(x => x.ProductId).IsUnique();

            builder.HasOne(x => x.Product).WithMany(x => x.ProductImages).HasForeignKey(x => x.ProductId);
        }
    }
}
