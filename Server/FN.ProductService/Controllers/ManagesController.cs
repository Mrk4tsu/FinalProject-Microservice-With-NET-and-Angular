using FN.Application.Catalog.Product;
using FN.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocelot.Values;

namespace FN.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ManagesController : BasesController
    {
        private readonly IProductManageService _service;
        public ManagesController(IProductManageService service)
        {
            this._service = service;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductPagingRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();
            var result = await _service.GetProducts(request, userId.Value);
            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId ==null)
            {
                return Unauthorized();
            }
            var result = await _service.Create(request, userId.Value);
            return Ok(result);
        }
    }
}
