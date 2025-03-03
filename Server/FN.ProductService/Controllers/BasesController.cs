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
        public static IActionResult Success(string? message = null, object data = null)
        {
            return new OkObjectResult(new
            {
                success = true,
                message,
                data
            });
        }
        public static IActionResult Success(object data = null)
        {
            return new OkObjectResult(new
            {
                success = true,
                data
            });
        }
        public static IActionResult Error(string? message = null, Code statusCode = Code.BadRequest, object errors = null)
        {
            return new ObjectResult(new
            {
                success = false,
                message,
                errors
            })
            {
                StatusCode = (int)statusCode
            };
        }
        public static IActionResult NotFound(string? message = null, object errors = null)
        {
            return new ObjectResult(new
            {
                success = false,
                message,
                errors
            })
            {
                StatusCode = Code.NotFound.GetHashCode()
            };
        }
        public static IActionResult BadRequest(string? message = null, object errors = null)
        {
            return new ObjectResult(new
            {
                success = false,
                message,
                errors
            })
            {
                StatusCode = Code.BadRequest.GetHashCode()
            };
        }
        public enum Code
        {
            Ok = 200,
            Created = 201,
            NoContent = 204,
            BadRequest = 400,
            Unauthorized = 401,
            Forbidden = 403,
            NotFound = 404,
            Conflict = 409,
            InternalServerError = 500,
            GatewayTimeout = 504
        }
    }
}
