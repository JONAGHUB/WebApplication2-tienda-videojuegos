using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers.API
{
    [Route("api/videojuegos")]
    [ApiController]
    public class VideojuegosApiController : ControllerBase
    {
        private readonly SistemaJuegosDbContext _db;

        public VideojuegosApiController(SistemaJuegosDbContext db)
        {
            _db = db;
        }

        // GET: api/videojuegos
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? genero = null,
            [FromQuery] string? plataforma = null,
            [FromQuery] int? limit = null)
        {
            var query = _db.Videojuegos.AsQueryable();

            if (!string.IsNullOrEmpty(genero))
                query = query.Where(v => v.Genero == genero);

            if (!string.IsNullOrEmpty(plataforma))
                query = query.Where(v => v.Plataforma == plataforma);

            if (limit.HasValue)
                query = query.Take(limit.Value);

            var games = await query.OrderBy(v => v.Titulo).ToListAsync();
            return Ok(games);
        }

        // GET: api/videojuegos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var game = await _db.Videojuegos.FindAsync(id);
            
            if (game == null)
                return NotFound(new { message = "Juego no encontrado" });

            return Ok(game);
        }

        // GET: api/videojuegos/search?q=halo
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(new { message = "Término de búsqueda requerido" });

            var games = await _db.Videojuegos
                .Where(v => v.Titulo!.Contains(q) || v.Descripcion!.Contains(q))
                .ToListAsync();

            return Ok(games);
        }

        // POST: api/videojuegos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Videojuego videojuego)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            videojuego.FechaCreacion = DateTime.Now;
            _db.Videojuegos.Add(videojuego);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = videojuego.Id }, videojuego);
        }

        // PUT: api/videojuegos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Videojuego videojuego)
        {
            if (id != videojuego.Id)
                return BadRequest(new { message = "ID no coincide" });

            _db.Entry(videojuego).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Videojuegos.AnyAsync(v => v.Id == id))
                    return NotFound();
                throw;
            }

            return Ok(new { message = "Juego actualizado exitosamente" });
        }

        // DELETE: api/videojuegos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _db.Videojuegos.FindAsync(id);
            if (game == null)
                return NotFound(new { message = "Juego no encontrado" });

            _db.Videojuegos.Remove(game);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Juego eliminado exitosamente" });
        }
    }
}