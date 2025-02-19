using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FN.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasesController : ControllerBase
    {
        protected int? GetUserIdFromClaims()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userId == null) return null;

            return int.Parse(userId);
        }
    }
}
