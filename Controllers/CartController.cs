using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CartController : Controller
    {
        private readonly SistemaJuegosDbContext _db;
        private const string SessionCartKey = "cart.items";

        public CartController(SistemaJuegosDbContext db)
        {
            _db = db;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();
            return View(cart);
        }

        // POST: /Cart/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id, int cantidad = 1)
        {
            var juego = await _db.Videojuegos.FindAsync(id);
            if (juego == null) return NotFound();

            // Convertir valores nullable a tipos seguros
            decimal precio = juego.Precio ?? 0m;
            string titulo = juego.Titulo ?? string.Empty;

            var cart = HttpContext.Session.GetObject<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();
            var existing = cart.FirstOrDefault(c => c.VideojuegoId == id);
            if (existing != null)
            {
                existing.Cantidad += cantidad;
            }
            else
            {
                cart.Add(new CartItem
                {
                    VideojuegoId = id,
                    Titulo = titulo,
                    Precio = precio,
                    Cantidad = cantidad
                });
            }

            HttpContext.Session.SetObject(SessionCartKey, cart);
            return RedirectToAction("Index", "Cart");
        }

        // POST: /Cart/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();
            cart.RemoveAll(c => c.VideojuegoId == id);
            HttpContext.Session.SetObject(SessionCartKey, cart);
            return RedirectToAction("Index");
        }
    }
}