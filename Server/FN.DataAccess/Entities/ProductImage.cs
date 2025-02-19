namespace FN.DataAccess.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string PublicId { get; set; }
        public string Caption { get; set; }
        public int ProductDetailId { get; set; }
        public string ImageUrl { get; set; }
        public ProductDetail ProductDetail { get; set; }
    }
}
