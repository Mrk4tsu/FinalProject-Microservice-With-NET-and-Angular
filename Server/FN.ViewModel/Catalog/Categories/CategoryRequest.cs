using Microsoft.AspNetCore.Http;

namespace FN.ViewModel.Catalog.Categories
{
    public class CategoryCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile? Image { get; set; } = null;
        public string Other { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
