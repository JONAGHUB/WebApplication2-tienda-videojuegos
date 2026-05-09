using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("reportes_ventas")]
    public class ReporteVenta
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("periodo")]
        public string? Periodo { get; set; } // "2024-01", "2024-02"

        [Column("total_ventas")]
        public decimal TotalVentas { get; set; }

        [Column("total_productos_vendidos")]
        public int TotalProductosVendidos { get; set; }

        [Column("ganancia_neta")]
        public decimal GananciaNeta { get; set; }

        [Column("costo_total")]
        public decimal CostoTotal { get; set; }

        [Column("fecha_generacion")]
        public DateTime FechaGeneracion { get; set; }
    }
}