using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioApiController : ControllerBase
    {
        private readonly SistemaJuegosDbContext _context;
        private readonly ILogger<InventarioApiController> _logger;

        public InventarioApiController(
            SistemaJuegosDbContext context,
            ILogger<InventarioApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/inventarioapi/analisis
        [HttpGet("analisis")]
        public async Task<ActionResult<object>> GetAnalisisInventario()
        {
            try
            {
                var videojuegos = await _context.Videojuegos.ToListAsync();
                var costos = await _context.CostosInventario
                    .Include(c => c.Videojuego)
                    .ToListAsync();

                var conStock = videojuegos.Where(v => v.Stock > 0).Count();
                var sinStock = videojuegos.Where(v => v.Stock == 0).Count();
                var stockBajo = videojuegos.Where(v => v.Stock > 0 && v.Stock < 10).Count();

                var costoTotal = costos.Sum(c => c.CostoAdquisicion * (c.Videojuego?.Stock ?? 0));
                var valorInventario = videojuegos.Sum(v => (v.Precio ?? 0) * (v.Stock ?? 0));

                return Ok(new
                {
                    TotalProductos = videojuegos.Count,
                    ConStock = conStock,
                    SinStock = sinStock,
                    StockBajo = stockBajo,
                    CostoTotal = costoTotal,
                    ValorInventario = valorInventario,
                    GananciaEstimada = valorInventario - costoTotal
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en análisis de inventario");
                return StatusCode(500, new { message = "Error en análisis" });
            }
        }

        // GET: api/inventarioapi/reporte-stock
        [HttpGet("reporte-stock")]
        public async Task<ActionResult<object>> GetReporteStock()
        {
            try
            {
                var reporte = await _context.Videojuegos
                    .Select(v => new
                    {
                        v.Id,
                        v.Titulo,
                        v.Stock,
                        v.Precio,
                        v.Calidad,
                        Estado = v.Stock == 0 ? "Sin Stock" : 
                                 v.Stock < 10 ? "Stock Bajo" : "Disponible",
                        ValorStock = (v.Precio ?? 0) * (v.Stock ?? 0)
                    })
                    .OrderBy(v => v.Stock)
                    .ToListAsync();

                return Ok(reporte);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar reporte");
                return StatusCode(500, new { message = "Error al generar reporte" });
            }
        }

        // POST: api/inventarioapi/calcular-precio
        [HttpPost("calcular-precio")]
        public async Task<ActionResult<object>> CalcularPrecio([FromBody] CostoInventario costo)
        {
            try
            {
                var existente = await _context.CostosInventario
                    .FirstOrDefaultAsync(c => c.VideojuegoId == costo.VideojuegoId);

                if (existente != null)
                {
                    existente.CostoAdquisicion = costo.CostoAdquisicion;
                    existente.MargenGanancia = costo.MargenGanancia;
                    existente.PrecioCalculado = costo.PrecioSugerido;
                    existente.FechaActualizacion = DateTime.UtcNow;
                }
                else
                {
                    costo.PrecioCalculado = costo.PrecioSugerido;
                    costo.FechaActualizacion = DateTime.UtcNow;
                    _context.CostosInventario.Add(costo);
                }

                // Actualizar precio del videojuego
                var videojuego = await _context.Videojuegos.FindAsync(costo.VideojuegoId);
                if (videojuego != null)
                {
                    videojuego.Precio = costo.PrecioSugerido;
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    CostoAdquisicion = costo.CostoAdquisicion,
                    MargenGanancia = costo.MargenGanancia,
                    PrecioSugerido = costo.PrecioSugerido,
                    Message = "Precio calculado y actualizado correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calcular precio");
                return StatusCode(500, new { message = "Error al calcular precio" });
            }
        }
    }
}