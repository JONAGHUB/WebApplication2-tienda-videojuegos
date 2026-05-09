using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly SistemaJuegosDbContext _db;

        public AdminController(SistemaJuegosDbContext db)
        {
            _db = db;
        }

        // GET: api/Admin/Inventory
        [HttpGet("Inventory")]
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

            return Ok(vm);
        }

        // POST: api/Admin/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] VideojuegoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var v = new Videojuego
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                Genero = model.Genero,
                Plataforma = model.Plataforma,
                Precio = model.Precio,
                ImagenUrl = model.ImagenUrl,
                Stock = model.Stock,
                PuntajePromedio = model.PuntajePromedio,
                FechaCreacion = DateTime.Now
            };

            _db.Videojuegos.Add(v);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Juego creado exitosamente", id = v.Id });
        }

        // GET: api/Admin/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var v = await _db.Videojuegos.FindAsync(id);
            if (v == null) 
                return NotFound(new { message = "Juego no encontrado" });

            var vm = new VideojuegoViewModel
            {
                Id = v.Id,
                Titulo = v.Titulo ?? string.Empty,
                Descripcion = v.Descripcion ?? string.Empty,
                Genero = v.Genero ?? string.Empty,
                Plataforma = v.Plataforma ?? string.Empty,
                Precio = v.Precio ?? 0m,
                ImagenUrl = v.ImagenUrl ?? string.Empty,
                Stock = v.Stock ?? 0,
                PuntajePromedio = v.PuntajePromedio ?? 0.0
            };

            return Ok(vm);
        }

        // PUT: api/Admin/Update
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] VideojuegoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var v = await _db.Videojuegos.FindAsync(model.Id);
            if (v == null)
                return NotFound(new { message = "Juego no encontrado" });

            v.Titulo = model.Titulo;
            v.Descripcion = model.Descripcion;
            v.Genero = model.Genero;
            v.Plataforma = model.Plataforma;
            v.Precio = model.Precio;
            v.ImagenUrl = model.ImagenUrl;
            v.Stock = model.Stock;
            v.PuntajePromedio = model.PuntajePromedio;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Juego actualizado exitosamente" });
        }

        // DELETE: api/Admin/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var v = await _db.Videojuegos.FindAsync(id);
            if (v == null)
                return NotFound(new { message = "Juego no encontrado" });

            _db.Videojuegos.Remove(v);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Juego eliminado exitosamente" });
        }
    }
}