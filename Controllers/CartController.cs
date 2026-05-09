using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly SistemaJuegosDbContext _db;
        private const string SessionCartKey = "cart.items";

        public CartController(SistemaJuegosDbContext db)
        {
            _db = db;
        }

        // GET: api/Cart
        [HttpGet]
        public IActionResult GetCart()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();
            return Ok(cart);
        }

        // POST: api/Cart/Add
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddToCartRequest request)
        {
            var juego = await _db.Videojuegos.FindAsync(request.Id);
            if (juego == null) 
                return NotFound(new { message = "Juego no encontrado" });

            decimal precio = juego.Precio ?? 0m;
            string titulo = juego.Titulo ?? string.Empty;

            var cart = HttpContext.Session.GetObject<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();
            var existing = cart.FirstOrDefault(c => c.VideojuegoId == request.Id);
            
            if (existing != null)
            {
                existing.Cantidad += request.Cantidad;
            }
            else
            {
                cart.Add(new CartItem
                {
                    VideojuegoId = request.Id,
                    Titulo = titulo,
                    Precio = precio,
                    Cantidad = request.Cantidad
                });
            }

            HttpContext.Session.SetObject(SessionCartKey, cart);
            return Ok(new { message = "Añadido al carrito", cart });
        }

        // POST: api/Cart/Remove
        [HttpPost("Remove")]
        public IActionResult Remove([FromBody] RemoveFromCartRequest request)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();
            cart.RemoveAll(c => c.VideojuegoId == request.Id);
            HttpContext.Session.SetObject(SessionCartKey, cart);
            return Ok(new { message = "Eliminado del carrito", cart });
        }

        // POST: api/Cart/Clear
        [HttpPost("Clear")]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove(SessionCartKey);
            return Ok(new { message = "Carrito vaciado" });
        }
    }

    public class AddToCartRequest
    {
        public int Id { get; set; }
        public int Cantidad { get; set; } = 1;
    }

    public class RemoveFromCartRequest
    {
        public int Id { get; set; }
    }
}