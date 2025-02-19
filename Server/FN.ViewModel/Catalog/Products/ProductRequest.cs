using FN.DataAccess.Entities;
using FN.DataAccess.Enums;
using FN.ViewModel.Helper.Paging;
using Microsoft.AspNetCore.Http;

namespace FN.ViewModel.Catalog.Products
{
    public class ProductPagingRequest : PagedList
    {
        public string? KeyWord { get; set; }
        public int? CategoryId { get; set; }
        public string? CategorySeoAlias { get; set; }
    }
    public class CreateProductRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public IFormFile Thumbnail { get; set; }
        public string SeoTitle { get; set; }
    }
}
