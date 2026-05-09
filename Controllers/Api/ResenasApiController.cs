using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResenasApiController : ControllerBase
    {
        private readonly SistemaJuegosDbContext _context;
        private readonly ILogger<ResenasApiController> _logger;

        public ResenasApiController(
            SistemaJuegosDbContext context,
            ILogger<ResenasApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/resenasapi/videojuego/5
        [HttpGet("videojuego/{videojuegoId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetResenasPorVideojuego(int videojuegoId)
        {
            try
            {
                var resenas = await _context.Resenas
                    .Where(r => r.VideojuegoId == videojuegoId)
                    .Include(r => r.Usuario)
                    .OrderByDescending(r => r.FechaCreacion)
                    .Select(r => new
                    {
                        r.Id,
                        r.Puntuacion,
                        r.Comentario,
                        r.FechaCreacion,
                        Usuario = r.Usuario != null ? r.Usuario.Nombre : "Anónimo"
                    })
                    .ToListAsync();

                return Ok(resenas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener reseñas del videojuego {videojuegoId}");
                return StatusCode(500, new { message = "Error al obtener reseñas" });
            }
        }

        // POST: api/resenasapi
        [HttpPost]
        public async Task<ActionResult<Resena>> CrearResena(Resena resena)
        {
            try
            {
                resena.FechaCreacion = DateTime.UtcNow;
                _context.Resenas.Add(resena);
                await _context.SaveChangesAsync();

                // Actualizar puntuación promedio del videojuego
                await ActualizarPuntuacionPromedio(resena.VideojuegoId);

                return CreatedAtAction(nameof(GetResenasPorVideojuego), 
                    new { videojuegoId = resena.VideojuegoId }, resena);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear reseña");
                return StatusCode(500, new { message = "Error al crear reseña" });
            }
        }

        private async Task ActualizarPuntuacionPromedio(int videojuegoId)
        {
            var videojuego = await _context.Videojuegos.FindAsync(videojuegoId);
            if (videojuego != null)
            {
                var promedio = await _context.Resenas
                    .Where(r => r.VideojuegoId == videojuegoId)
                    .AverageAsync(r => (double)r.Puntuacion);

                var totalResenas = await _context.Resenas
                    .CountAsync(r => r.VideojuegoId == videojuegoId);

                videojuego.PuntajePromedio = promedio;
                videojuego.TotalResenas = totalResenas;
                await _context.SaveChangesAsync();
            }
        }
    }
}