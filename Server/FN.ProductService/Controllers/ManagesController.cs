using FN.Application.Catalog.Product;
using FN.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FN.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ManagesController : BasesController
    {
        private readonly IProductManageService service;
        public ManagesController(IProductManageService service)
        {
            this.service = service;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId ==null)
            {
                return Unauthorized();
            }
            var result = await service.Create(request, userId.Value);
            return Ok(result);
        }
    }
}
