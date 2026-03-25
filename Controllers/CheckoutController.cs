using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly SistemaJuegosDbContext _db;
        private const string SessionCartKey = "cart.items";

        public CheckoutController(SistemaJuegosDbContext db)
        {
            _db = db;
        }

        // GET: /Checkout
        // Mapa simple a VideojuegoViewModel para la vista resumen
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();
            var vm = cart.Select(c => new VideojuegoViewModel
            {
                Id = c.VideojuegoId,
                Titulo = c.Titulo,
                Precio = c.Precio,
                Stock = 0
            }).ToList();

            return View(vm);
        }

        // POST: /Checkout/Confirm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(string nombre, string direccion, string metodo)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(SessionCartKey) ?? new List<CartItem>();
            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in cart)
                {
                    var juego = await _db.Videojuegos.FindAsync(item.VideojuegoId);
                    if (juego == null) continue;

                    // Convertir nullable a valores seguros
                    decimal precio = juego.Precio ?? 0m;
                    int stockActual = juego.Stock ?? 0;

                    // Reducir stock si aplica
                    juego.Stock = Math.Max(0, stockActual - item.Cantidad);

                    var compra = new Compra
                    {
                        UsuarioId = 0, // asignar usuario real
                        VideojuegoId = juego.Id,
                        PrecioCompra = precio,
                        Cantidad = item.Cantidad,
                        Total = precio * item.Cantidad,
                        CodigoQr = string.Empty,
                        Fecha = DateTime.UtcNow
                    };

                    _db.Compras.Add(compra);
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                // Vaciar carrito
                HttpContext.Session.Remove(SessionCartKey);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                await tx.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Error al procesar la compra.");
                return RedirectToAction("Index");
            }
        }
    }
}