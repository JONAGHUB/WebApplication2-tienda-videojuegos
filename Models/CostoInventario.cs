using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("costos_inventario")]
    public class CostoInventario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("videojuego_id")]
        public int VideojuegoId { get; set; }

        [Column("costo_adquisicion")]
        public decimal CostoAdquisicion { get; set; }

        [Column("margen_ganancia")]
        [Range(0, 100)]
        public decimal MargenGanancia { get; set; } // Porcentaje

        [Column("precio_calculado")]
        public decimal PrecioCalculado { get; set; }

        [Column("stock_minimo")]
        public int StockMinimo { get; set; }

        [Column("stock_maximo")]
        public int StockMaximo { get; set; }

        [Column("fecha_actualizacion")]
        public DateTime FechaActualizacion { get; set; }

        [NotMapped]
        public decimal PrecioSugerido => CostoAdquisicion * (1 + MargenGanancia / 100);

        // Navegación
        [ForeignKey("VideojuegoId")]
        public virtual Videojuego? Videojuego { get; set; }
    }
}