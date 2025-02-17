namespace FN.DataAccess.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string NormalizeName { get; set; } = string.Empty;
        public string SeoTitle { get; set; } = string.Empty;
        public string SeoAlias { get; set; } = string.Empty;
        public string SeoDescription { get; set; } = string.Empty;
        public string SeoKeyword { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public Category Category { get; set; } = new Category();
        public AppUser User { get; set; } = new AppUser();
        public ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}
