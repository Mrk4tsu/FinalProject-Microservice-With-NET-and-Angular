using FN.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FN.DataAccess.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("blogs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Detail).IsRequired();
            builder.Property(x => x.LikeCount).HasDefaultValue(0);
            builder.Property(x => x.DislikeCount).HasDefaultValue(0);
            builder.HasOne(x => x.Item).WithMany(x => x.Blogs).HasForeignKey(x => x.ItemId);
        }
    }
}
