using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication2.Data;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly SistemaJuegosDbContext _db;
        private const string SessionCartKey = "cart.items";

        public CheckoutController(SistemaJuegosDbContext db)
        {
            _db = db;
        }

        // GET: api/Checkout
        [HttpGet]
        public IActionResult GetCheckoutData()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();

            if (!cart.Any())
                return BadRequest(new { message = "El carrito está vacío" });

            var total = cart.Sum(item => item.Precio * item.Cantidad);

            return Ok(new { cart, total });
        }

        // POST: api/Checkout/Process
        [HttpPost("Process")]
        public async Task<IActionResult> Process()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();

            if (!cart.Any())
                return BadRequest(new { message = "El carrito está vacío" });

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            int usuarioId = int.Parse(userIdClaim);

            foreach (var item in cart)
            {
                var compra = new Compra
                {
                    UsuarioId = usuarioId,
                    VideojuegoId = item.VideojuegoId,
                    Cantidad = item.Cantidad,
                    PrecioCompra = item.Precio,
                    Total = item.Precio * item.Cantidad,
                    Fecha = DateTime.Now,
                    CodigoQr = string.Empty
                };
                _db.Compras.Add(compra);

                var juego = await _db.Videojuegos.FindAsync(item.VideojuegoId);
                if (juego != null && juego.Stock.HasValue)
                {
                    juego.Stock -= item.Cantidad;
                }
            }

            await _db.SaveChangesAsync();
            HttpContext.Session.Remove(SessionCartKey);

            return Ok(new { message = "Compra realizada exitosamente" });
        }
    }
}