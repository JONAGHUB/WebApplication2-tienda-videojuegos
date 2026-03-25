using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AdminController : Controller
    {
        private readonly SistemaJuegosDbContext _db;

        public AdminController(SistemaJuegosDbContext db)
        {
            _db = db;
        }

        // GET: /Admin/Inventory
        public async Task<IActionResult> Inventory()
        {
            var list = await _db.Videojuegos.OrderBy(v => v.Titulo).ToListAsync();
            var vm = list.Select(v => new VideojuegoViewModel
            {
                Id = v.Id,
                Titulo = v.Titulo ?? string.Empty,
                Descripcion = v.Descripcion ?? string.Empty,
                Plataforma = v.Plataforma ?? string.Empty,
                Genero = v.Genero ?? string.Empty,
                Precio = v.Precio ?? 0m,
                ImagenUrl = string.IsNullOrWhiteSpace(v.ImagenUrl) ? "/images/placeholder.png" : v.ImagenUrl!,
                Stock = v.Stock ?? 0,
                PuntajePromedio = v.PuntajePromedio ?? 0.0
            });

            return View(vm);
        }

        // GET: /Admin/Create
        public IActionResult Create()
        {
            return View("CreateEdit", new VideojuegoViewModel());
        }

        // GET: /Admin/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var v = await _db.Videojuegos.FindAsync(id);
            if (v == null) return NotFound();

            var vm = new VideojuegoViewModel
            {
                Id = v.Id,
                Titulo = v.Titulo ?? string.Empty,
                Descripcion = v.Descripcion ?? string.Empty,
                Plataforma = v.Plataforma ?? string.Empty,
                Genero = v.Genero ?? string.Empty,
                Precio = v.Precio ?? 0m,
                ImagenUrl = string.IsNullOrWhiteSpace(v.ImagenUrl) ? "/images/placeholder.png" : v.ImagenUrl!,
                Stock = v.Stock ?? 0,
                PuntajePromedio = v.PuntajePromedio ?? 0.0
            };

            return View("CreateEdit", vm);
        }

        // POST: /Admin/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(VideojuegoViewModel model)
        {
            if (!ModelState.IsValid) return View("CreateEdit", model);

            if (model.Id > 0)
            {
                var exist = await _db.Videojuegos.FindAsync(model.Id);
                if (exist == null) return NotFound();

                exist.Titulo = model.Titulo;
                exist.Descripcion = model.Descripcion;
                exist.Genero = model.Genero;
                exist.Plataforma = model.Plataforma;
                exist.Precio = model.Precio;
                exist.ImagenUrl = model.ImagenUrl;
                exist.Stock = model.Stock;
                exist.PuntajePromedio = model.PuntajePromedio;
                await _db.SaveChangesAsync();
            }
            else
            {
                var nuevo = new Videojuego
                {
                    Titulo = model.Titulo,
                    Descripcion = model.Descripcion,
                    Genero = model.Genero,
                    Plataforma = model.Plataforma,
                    Precio = model.Precio,
                    ImagenUrl = model.ImagenUrl,
                    Stock = model.Stock,
                    PuntajePromedio = model.PuntajePromedio,
                    FechaCreacion = DateTime.UtcNow
                };
                _db.Videojuegos.Add(nuevo);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Inventory");
        }

        // POST: /Admin/Restock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restock(int id, int cantidad = 10)
        {
            var v = await _db.Videojuegos.FindAsync(id);
            if (v == null) return NotFound();

            v.Stock = (v.Stock ?? 0) + cantidad;
            await _db.SaveChangesAsync();

            return RedirectToAction("Inventory");
        }
    }
}