namespace FN.ViewModel.Catalog.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public string Author { get; set; } = string.Empty;
        public DateTime UploadTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Version { get; set; } = string.Empty;
        public string SeoAlias { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
    }
}
