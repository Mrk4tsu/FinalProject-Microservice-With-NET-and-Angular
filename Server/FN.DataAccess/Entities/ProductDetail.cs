using FN.DataAccess.Enums;

namespace FN.DataAccess.Entities
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Detail { get; set; } = string.Empty;
        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }
        public int DownloadCount { get; set; }
        public string Version { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public ProductType ProductType { get; set; } = ProductType.PUBLIC;
        public bool Status { get; set; }
        public Item Item { get; set; } = new Item();
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
    }
}
