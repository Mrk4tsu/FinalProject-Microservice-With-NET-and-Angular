namespace FN.DataAccess.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NormalizeName { get; set; } = string.Empty;
        public string SeoTitle { get; set; } = string.Empty;
        public string SeoAlias { get; set; } = string.Empty;
        public string SeoDescription { get; set; } = string.Empty;
        public string SeoKeyword { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Other { get; set; } = string.Empty;
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
