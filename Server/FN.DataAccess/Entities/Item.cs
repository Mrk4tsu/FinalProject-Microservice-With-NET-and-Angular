namespace FN.DataAccess.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string NormalizedTitle { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Thumbnail { get; set; }
        public int ViewCount { get; set; }
        public string SeoAlias { get; set; }
        public string SeoTitle { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public AppUser User { get; set; }
        public List<ProductDetail> ProductDetails { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
