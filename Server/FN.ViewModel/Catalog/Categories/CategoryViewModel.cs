namespace FN.ViewModel.Catalog.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Other { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string SeoAlias { get; set; } = string.Empty;
    }
    public class CategoryDetailViewModel : CategoryViewModel
    {
        public string SeoTitle { get; set; } = string.Empty;
        public string SeoDescription { get; set; } = string.Empty;
    }
}
