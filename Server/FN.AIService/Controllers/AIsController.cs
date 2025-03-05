using GeminiAIDev.Client;
using GeminiAIDev.Models;
using Microsoft.AspNetCore.Mvc;

namespace FN.AIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIsController : ControllerBase
    {
        private readonly GeminiApiClient apiClient;
        public AIsController(GeminiApiClient apiClient)
        {
            this.apiClient = apiClient;
        }
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateContent([FromBody] PromptRequest request)
        {
            try
            {
                string content = await apiClient.GenerateContentAsync(request.Prompt);
                return Ok(content);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("description")]
        public async Task<IActionResult> GenerateDescription([FromBody] PromptDescriptionRequest request)
        {
            try
            {
                var prompt = $"hãy cho tôi 1 mô tả tổng quát cho sản phẩm lập trình có tên {request.Name} để làm mô tả cho phần SEO với các đặc điểm: Dành cho người dùng {request.Other}, hệ điều hành {request.System}. Hãy vào vấn đề chính, không lan man";
                string content = await apiClient.GenerateContentAsync(prompt);
                return Ok(content);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("create-game-news")]
        public async Task<IActionResult> CreateGameNews()
        {
            try
            {
                var postResponse = await apiClient.CreatePostAsync();
                return Ok(postResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}