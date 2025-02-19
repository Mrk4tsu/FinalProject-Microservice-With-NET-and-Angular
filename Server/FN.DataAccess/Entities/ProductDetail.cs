using FN.DataAccess.Enums;

namespace FN.DataAccess.Entities
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public string Detail { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int DownloadCount { get; set; }
        public string Version { get; set; }
        public string Note { get; set; }
        public int ItemId { get; set; }
        public byte CategoryId { get; set; }
        public ProductType Status { get; set; }
        public bool IsDeleted { get; set; }
        public Item Item { get; set; }
        public Category Category { get; set; }
        public List<ProductPrice> ProductPrices { get; set; }
    }
}
