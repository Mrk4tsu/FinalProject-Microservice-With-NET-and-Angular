using FN.ViewModel.Helper.Paging;

namespace FN.ViewModel.Catalog.Products
{
    public class ProductPaginhRequest : PagedList
    {
        public string? KeyWord { get; set; }
        public int? CategoryId { get; set; }
        public string? CategorySeoAlias { get; set; }
    }
}
