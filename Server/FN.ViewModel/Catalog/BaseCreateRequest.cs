using Microsoft.AspNetCore.Http;

namespace FN.ViewModel.Catalog
{
    public class BaseCreateRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public IFormFile Thumbnail { get; set; }
    }
}
