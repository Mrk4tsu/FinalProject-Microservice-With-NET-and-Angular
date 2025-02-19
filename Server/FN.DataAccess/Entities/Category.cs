namespace FN.DataAccess.Entities
{
    public class Category
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string SeoAlias { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeyword { get; set; }
        public string SeoImage { get; set; }
        public string Other { get; set; }
        public bool Status { get; set; }
        public List<ProductDetail> ProductDetails { get; set; }
    }
}
