using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers.API
{
    [Route("api/compras")]
    [ApiController]
    [Authorize]
    public class ComprasApiController : ControllerBase
    {
        private readonly SistemaJuegosDbContext _db;

        public ComprasApiController(SistemaJuegosDbContext db)
        {
            _db = db;
        }

        // POST: api/compras
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Usuario no autenticado" });

            var usuarioId = int.Parse(userIdClaim);

            if (request.Items == null || !request.Items.Any())
                return BadRequest(new { message = "El carrito está vacío" });

            var comprasCreadas = new List<object>();

            // Crear compras para cada item
            foreach (var item in request.Items)
            {
                var juego = await _db.Videojuegos.FindAsync(item.VideojuegoId);
                if (juego == null)
                    return BadRequest(new { message = $"Juego con ID {item.VideojuegoId} no encontrado" });

                if (juego.Stock < item.Cantidad)
                    return BadRequest(new { message = $"Stock insuficiente para {juego.Titulo}" });

                var compra = new Compra
                {
                    UsuarioId = usuarioId,
                    VideojuegoId = item.VideojuegoId,
                    Cantidad = item.Cantidad,
                    PrecioCompra = item.PrecioUnitario,
                    Total = item.Cantidad * item.PrecioUnitario,
                    Fecha = DateTime.Now,
                    CodigoQr = string.Empty
                };

                _db.Compras.Add(compra);

                // Actualizar stock
                juego.Stock -= item.Cantidad;

                comprasCreadas.Add(new
                {
                    compra.Id,
                    juego = juego.Titulo,
                    cantidad = compra.Cantidad,
                    total = compra.Total
                });
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Compra realizada exitosamente",
                compras = comprasCreadas,
                totalGeneral = request.Total
            });
        }

        // GET: api/compras
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Usuario no autenticado" });

            var usuarioId = int.Parse(userIdClaim);

            var compras = await _db.Compras
                .Include(c => c.Videojuego)
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.Fecha)
                .Select(c => new
                {
                    id = c.Id,
                    fecha = c.Fecha,
                    total = c.Total,
                    cantidad = c.Cantidad,
                    juego = c.Videojuego!.Titulo,
                    estado = "Completado"
                })
                .ToListAsync();

            return Ok(compras);
        }

        // GET: api/compras/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var usuarioId = int.Parse(userIdClaim);

            var compra = await _db.Compras
                .Include(c => c.Videojuego)
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == usuarioId);

            if (compra == null)
                return NotFound(new { message = "Compra no encontrada" });

            return Ok(new
            {
                id = compra.Id,
                fecha = compra.Fecha,
                total = compra.Total,
                cantidad = compra.Cantidad,
                juego = compra.Videojuego!.Titulo,
                estado = "Completado"
            });
        }
    }

    public class OrderRequest
    {
        public int UsuarioId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal Total { get; set; }
        public string DireccionEnvio { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;
    }

    public class OrderItem
    {
        public int VideojuegoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}