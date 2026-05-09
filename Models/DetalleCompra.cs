using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("DetalleCompras")]
    public class DetalleCompra
    {
        [Key]
        public int Id { get; set; }

        public int CompraId { get; set; }

        public int VideojuegoId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navegación
        public Compra? Compra { get; set; }
        public Videojuego? Videojuego { get; set; }
    }
}