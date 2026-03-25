using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers.Api
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "1234")
                return Ok("Login correcto");

            return Unauthorized();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}