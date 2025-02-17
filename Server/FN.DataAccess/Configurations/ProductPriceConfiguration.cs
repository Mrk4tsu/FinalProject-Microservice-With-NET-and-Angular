using FN.DataAccess.Entities;
using FN.DataAccess.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FN.DataAccess.Configurations
{
    public class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
    {
        public void Configure(EntityTypeBuilder<ProductPrice> builder)
        {
            builder.ToTable("product_prices");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Price).IsRequired().HasPrecision(18, 2).HasDefaultValue(0);
            builder.Property(x => x.PriceType).HasDefaultValue(PriceType.BASE);
            builder.Property(x => x.StartDate).HasDefaultValue(DateTime.UtcNow);
            builder.Property(x => x.EndDate).HasDefaultValue(DateTime.UtcNow);
            builder.Property(x => x.CreatedDate).HasDefaultValue(DateTime.UtcNow);

            builder.HasIndex(x => x.ProductId);
            builder.HasOne(x => x.Product).WithMany(x => x.ProductPrices).HasForeignKey(x => x.ProductId);
        }
    }
}
