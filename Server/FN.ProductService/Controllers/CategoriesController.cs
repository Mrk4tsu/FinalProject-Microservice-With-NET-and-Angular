using FN.Application.Catalog.Categories;
using FN.ViewModel.Catalog.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FN.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController, AllowAnonymous]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var result = await _categoryService.List();
            if (result.Success)
                return Ok(result);
            return BadRequest();
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateUpdateRequest request)
        {
            var result = await _categoryService.Create(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Message);
        }
        [HttpPut("update"), AllowAnonymous]
        public async Task<IActionResult> Update([FromForm] CategoryCreateUpdateRequest request, byte categoryId)
        {
            var result = await _categoryService.Update(request, categoryId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Message);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(byte categoryId)
        {
            var result = await _categoryService.Delete(categoryId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Message);
        }
        [HttpDelete("permanently-delete")]
        public async Task<IActionResult> PermanentlyDelete(byte categoryId)
        {
            var result = await _categoryService.PermanentlyDelete(categoryId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Message);
        }
    }
}
