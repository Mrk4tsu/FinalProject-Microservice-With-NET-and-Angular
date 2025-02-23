using FN.Application.Catalog.Product;
using FN.Application.Helper.Images;
using FN.ViewModel.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocelot.Values;

namespace FN.ProductService.Controllers
{
    [Route("api/manage")]
    [ApiController, Authorize]
    public class ManagesController : BasesController
    {
        private readonly IProductManageService _service;
        private readonly IImageService _imageService;
        public ManagesController(IProductManageService service, IImageService imageService)
        {
            this._service = service;
            this._imageService = imageService;
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
            if (userId == null)
            {
                return Unauthorized();
            }
            var result = await _service.Create(request, userId.Value);
            return Ok(result);
        }
        [HttpPut("update{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ItemUpdateRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var result = await _service.Update(request, id, userId.Value);
            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }
        [HttpPost("up-img"), AllowAnonymous]
        public async Task<IActionResult> UploadImage([FromForm] List<IFormFile> file, string folder)
        {
            var result = await _imageService.UploadImages(file, folder);
            if (result == null)
            {
                return Error("Upload image failed");
            }
            return Success(result);
        }
    }
}
