using System.Text.Json.Serialization;

namespace FN.ViewModel.Catalog.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string NormalizeTitle { get; set; }
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }
        public string SeoAlias { get; set; }
        public string Username { get; set; }
        public string Version { get; set; }
        public int DownloadCount { get; set; }
        public string CategoryIcon { get; set; }
        public string CategorySeoAlias { get; set; }
        public DateTime TimeCreates { get; set; }
        public DateTime TimeUpdates { get; set; }
    }
    public class ProductDetailViewModel : ProductViewModel
    {
        public string Detail { get; set; }
        public string Note { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        [JsonIgnore]
        public string? ImagesJson { get; set; }
        public List<ImageProductViewModel> Images { get; set; }
    }
    public class ImageProductViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Caption { get; set; }
    }
}
