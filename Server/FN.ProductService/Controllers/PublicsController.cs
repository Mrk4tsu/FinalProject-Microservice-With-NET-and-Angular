using FN.Application.Catalog.Product;
using FN.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FN.ProductService.Controllers
{
    [Route("api/public")]
    [ApiController, AllowAnonymous]
    public class PublicsController : ControllerBase
    {
        private readonly IProductPublicService _service;
        public PublicsController(IProductPublicService service)
        {
            _service = service;
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductPagingRequest request)
        {
            var products = await _service.GetProducts(request);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _service.GetProduct(id);
            return Ok(product);
        }
    }
}
