using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using System.Security.Claims;

namespace WebApplication2.Controllers
{
    public class GamesController : Controller
    {
        private readonly SistemaJuegosDbContext _db;
        private const int DefaultPageSize = 12;

        public GamesController(SistemaJuegosDbContext db)
        {
            _db = db;
        }

        // GET: /Games?page=1&pageSize=12&q=texto
        public async Task<IActionResult> Index(string? q, int page = 1, int pageSize = DefaultPageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = DefaultPageSize;

            var baseQuery = _db.Videojuegos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                baseQuery = baseQuery.Where(v =>
                    (v.Titulo ?? string.Empty).Contains(q) ||
                    (v.Genero ?? string.Empty).Contains(q) ||
                    (v.Plataforma ?? string.Empty).Contains(q));
            }

            var total = await baseQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            var items = await baseQuery
                .OrderBy(v => v.Titulo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var vm = items.Select(v => new VideojuegoViewModel
            {
                Id = v.Id,
                Titulo = v.Titulo ?? string.Empty,
                Descripcion = v.Descripcion ?? string.Empty,
                Genero = v.Genero ?? string.Empty,
                Plataforma = v.Plataforma ?? string.Empty,
                Precio = v.Precio ?? 0m,
                ImagenUrl = string.IsNullOrWhiteSpace(v.ImagenUrl) ? "/images/placeholder.png" : v.ImagenUrl!,
                VideoUrl = v.VideoUrl ?? string.Empty,
                Stock = v.Stock ?? 0,
                PuntajePromedio = v.PuntajePromedio ?? 0.0
            }).ToList();

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Query = q ?? string.Empty;
            ViewBag.PageSize = pageSize;

            return View(vm);
        }

        // GET: /Games/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.Videojuegos.FindAsync(id);
            if (item == null) return NotFound();

            var vm = new VideojuegoViewModel
            {
                Id = item.Id,
                Titulo = item.Titulo ?? string.Empty,
                Descripcion = item.Descripcion ?? string.Empty,
                Genero = item.Genero ?? string.Empty,
                Plataforma = item.Plataforma ?? string.Empty,
                Precio = item.Precio ?? 0m,
                ImagenUrl = string.IsNullOrWhiteSpace(item.ImagenUrl) ? "/images/placeholder.png" : item.ImagenUrl!,
                Stock = item.Stock ?? 0,
                PuntajePromedio = item.PuntajePromedio ?? 0.0
            };

            // Cargar reseñas y nombres (manejar nulls) — usar DbSet Resenas (modelo Resena)
            var resenas = await _db.Resenas
                .Where(r => r.VideojuegoId == id)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();

            var reviewVms = new List<ReviewViewModel>();
            foreach (var r in resenas)
            {
                string usuarioNombre = "Anónimo";
                if (r.UsuarioId != 0)
                {
                    var usr = await _db.Usuarios.FindAsync(r.UsuarioId);
                    if (usr != null)
                        usuarioNombre = string.IsNullOrWhiteSpace(usr.Nombre) ? usr.Correo : usr.Nombre;
                }

                reviewVms.Add(new ReviewViewModel
                {
                    Calificacion = r.Puntuacion,
                    Comentario = r.Comentario ?? string.Empty,
                    Fecha = r.FechaCreacion,
                    UsuarioNombre = usuarioNombre
                });
            }

            // Media calculada (si hay reseñas usarlas, si no fallback a PuntajePromedio guardado)
            double avg = reviewVms.Any() ? reviewVms.Average(x => x.Calificacion) : vm.PuntajePromedio;

            ViewBag.Reviews = reviewVms;
            ViewBag.Average = avg;

            return View(vm);
        }

        // POST: /Games/Rate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rate(int id, int score, string? comment)
        {
            if (score < 1 || score > 5)
            {
                ModelState.AddModelError(string.Empty, "Puntuación inválida.");
                return RedirectToAction("Details", new { id });
            }

            var juego = await _db.Videojuegos.FindAsync(id);
            if (juego == null) return NotFound();

            // Usar el modelo Resena (sin tilde) y sus propiedades reales
            var resena = new Resena
            {
                VideojuegoId = id,
                UsuarioId = User?.Identity?.IsAuthenticated == true ? GetCurrentUserId() : 0,
                Puntuacion = score,
                Comentario = comment ?? string.Empty,
                FechaCreacion = DateTime.UtcNow
            };

            _db.Resenas.Add(resena);
            await _db.SaveChangesAsync();

            // Recalcular puntaje promedio seguro (usar DbSet Resenas)
            var avg = await _db.Resenas
                .Where(r => r.VideojuegoId == id)
                .AverageAsync(r => (double?)r.Puntuacion) ?? score;

            juego.PuntajePromedio = avg;
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", new { id });
        }

        // Helper: obtener user id (si tienes autenticación real sustituir)
        private int GetCurrentUserId()
        {
            var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(idClaim, out var id))
                return id;
            return 0;
        }
    }
}