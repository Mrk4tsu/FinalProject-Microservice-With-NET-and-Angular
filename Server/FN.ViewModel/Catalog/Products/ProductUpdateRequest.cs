using FN.DataAccess.Entities;
using FN.DataAccess.Enums;
using Microsoft.AspNetCore.Http;

namespace FN.ViewModel.Catalog.Products
{
    public class ItemUpdateRequest : ProductDeatilUpdateRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Keywords { get; set; }
        public IFormFile? Thumbnail { get; set; }
    }
    public class ProductDeatilUpdateRequest
    {
        public string Detail { get; set; }
        public string Version { get; set; }
        public string Note { get; set; }
        public byte CategoryId { get; set; }
        public ProductType Status { get; set; }
    }
}
