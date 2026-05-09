using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;

namespace WebApplication2.Controllers.API
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminApiController : ControllerBase
    {
        private readonly SistemaJuegosDbContext _db;

        public AdminApiController(SistemaJuegosDbContext db)
        {
            _db = db;
        }

        // GET: api/admin/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = new
            {
                totalGames = await _db.Videojuegos.CountAsync(),
                totalUsers = await _db.Usuarios.CountAsync(),
                totalOrders = await _db.Compras.CountAsync(),
                totalRevenue = await _db.Compras.SumAsync(c => c.Total)
            };

            return Ok(stats);
        }

        // GET: api/admin/usuarios
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _db.Usuarios
                .Select(u => new
                {
                    u.Id,
                    u.Nombre,
                    u.Correo,
                    u.Rol,
                    u.FechaCreacion
                })
                .OrderBy(u => u.Nombre)
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/admin/compras
        [HttpGet("compras")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _db.Compras
                .Include(c => c.Usuario)
                .Include(c => c.Videojuego)
                .Select(c => new
                {
                    id = c.Id,
                    usuario = c.Usuario!.Nombre,
                    juego = c.Videojuego!.Titulo,
                    cantidad = c.Cantidad,
                    total = c.Total,
                    fecha = c.Fecha
                })
                .OrderByDescending(c => c.fecha)
                .ToListAsync();

            return Ok(orders);
        }
    }
}