using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SistemaJuegosDbContext _db;

        public AccountController(SistemaJuegosDbContext db)
        {
            _db = db;
        }

        // POST: api/Account/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // DEBUG: Ver qué usuarios existen
            var todosUsuarios = await _db.Usuarios.ToListAsync();
            Console.WriteLine($"Usuarios en BD: {todosUsuarios.Count}");
            foreach (var u in todosUsuarios)
            {
                Console.WriteLine($"- {u.Correo} / {u.Contrasena}");
            }

            var usuario = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == model.Correo && u.Contrasena == model.Contrasena);

            if (usuario == null)
            {
                Console.WriteLine($"Login fallido para: {model.Correo} / {model.Contrasena}");
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            var name = string.IsNullOrWhiteSpace(usuario.Nombre) ? usuario.Correo : usuario.Nombre;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol ?? string.Empty)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(new
            {
                id = usuario.Id,
                nombre = usuario.Nombre,
                correo = usuario.Correo,
                rol = usuario.Rol
            });
        }

        // POST: api/Account/Logout
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Sesión cerrada" });
        }
    }
}
