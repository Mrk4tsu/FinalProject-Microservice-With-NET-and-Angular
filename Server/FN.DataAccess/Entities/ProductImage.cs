namespace FN.DataAccess.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string PublicId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string UrlImage { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
        public ProductDetail Product { get; set; } = new ProductDetail();
    }
}
