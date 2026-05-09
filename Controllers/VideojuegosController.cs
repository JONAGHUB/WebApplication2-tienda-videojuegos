using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideojuegosApiController : ControllerBase
    {
        private readonly SistemaJuegosDbContext _context;
        private readonly ILogger<VideojuegosApiController> _logger;

        public VideojuegosApiController(
            SistemaJuegosDbContext context,
            ILogger<VideojuegosApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/videojuegosapi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Videojuego>>> GetVideojuegos(
            [FromQuery] string? genero = null,
            [FromQuery] string? plataforma = null,
            [FromQuery] int? limit = null)
        {
            try
            {
                var query = _context.Videojuegos.AsQueryable();

                // Filtrar por género si se proporciona
                if (!string.IsNullOrEmpty(genero))
                {
                    query = query.Where(v => v.Genero == genero);
                }

                // Filtrar por plataforma si se proporciona
                if (!string.IsNullOrEmpty(plataforma))
                {
                    query = query.Where(v => v.Plataforma != null && v.Plataforma.Contains(plataforma));
                }

                // Ordenar por título (alfabéticamente)
                query = query.OrderBy(v => v.Titulo);

                // Limitar resultados si se especifica
                if (limit.HasValue && limit.Value > 0)
                {
                    query = query.Take(limit.Value);
                }

                var videojuegos = await query.ToListAsync();
                
                _logger.LogInformation($"Devolviendo {videojuegos.Count} videojuegos");
                
                return Ok(videojuegos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener videojuegos");
                return StatusCode(500, new { message = "Error al obtener videojuegos", error = ex.Message });
            }
        }

        // GET: api/videojuegosapi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Videojuego>> GetVideojuego(int id)
        {
            try
            {
                var videojuego = await _context.Videojuegos.FindAsync(id);

                if (videojuego == null)
                {
                    _logger.LogWarning($"Videojuego con ID {id} no encontrado");
                    return NotFound(new { message = $"Videojuego con ID {id} no encontrado" });
                }

                return Ok(videojuego);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener videojuego con ID {id}");
                return StatusCode(500, new { message = "Error al obtener el videojuego", error = ex.Message });
            }
        }

        // GET: api/videojuegosapi/search?q=termino
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Videojuego>>> SearchVideojuegos([FromQuery] string q)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                {
                    // Si no hay término de búsqueda, devolver todos
                    return await GetVideojuegos();
                }

                var searchTerm = q.ToLower();
                
                var videojuegos = await _context.Videojuegos
                    .Where(v => 
                        (v.Titulo != null && v.Titulo.ToLower().Contains(searchTerm)) ||
                        (v.Descripcion != null && v.Descripcion.ToLower().Contains(searchTerm)) ||
                        (v.Genero != null && v.Genero.ToLower().Contains(searchTerm)) ||
                        (v.Desarrolladora != null && v.Desarrolladora.ToLower().Contains(searchTerm)))
                    .OrderBy(v => v.Titulo)
                    .ToListAsync();

                _logger.LogInformation($"Búsqueda '{q}' devolvió {videojuegos.Count} resultados");
                
                return Ok(videojuegos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar videojuegos con término: {q}");
                return StatusCode(500, new { message = "Error en la búsqueda", error = ex.Message });
            }
        }

        // POST: api/videojuegosapi
        [HttpPost]
        public async Task<ActionResult<Videojuego>> CreateVideojuego(Videojuego videojuego)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                videojuego.FechaCreacion = DateTime.UtcNow;
                _context.Videojuegos.Add(videojuego);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Videojuego creado: {videojuego.Titulo} (ID: {videojuego.Id})");

                return CreatedAtAction(nameof(GetVideojuego), new { id = videojuego.Id }, videojuego);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear videojuego");
                return StatusCode(500, new { message = "Error al crear el videojuego", error = ex.Message });
            }
        }

        // PUT: api/videojuegosapi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideojuego(int id, Videojuego videojuego)
        {
            if (id != videojuego.Id)
            {
                return BadRequest(new { message = "El ID no coincide con el videojuego" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(videojuego).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Videojuego actualizado: ID {id}");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideojuegoExists(id))
                {
                    _logger.LogWarning($"Videojuego con ID {id} no encontrado al actualizar");
                    return NotFound(new { message = $"Videojuego con ID {id} no encontrado" });
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar videojuego con ID {id}");
                return StatusCode(500, new { message = "Error al actualizar el videojuego", error = ex.Message });
            }
        }

        // DELETE: api/videojuegosapi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideojuego(int id)
        {
            try
            {
                var videojuego = await _context.Videojuegos.FindAsync(id);
                if (videojuego == null)
                {
                    _logger.LogWarning($"Videojuego con ID {id} no encontrado al eliminar");
                    return NotFound(new { message = $"Videojuego con ID {id} no encontrado" });
                }

                _context.Videojuegos.Remove(videojuego);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Videojuego eliminado: {videojuego.Titulo} (ID: {id})");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar videojuego con ID {id}");
                return StatusCode(500, new { message = "Error al eliminar el videojuego", error = ex.Message });
            }
        }

        // Método auxiliar para verificar existencia
        private bool VideojuegoExists(int id)
        {
            return _context.Videojuegos.Any(e => e.Id == id);
        }
    }
}