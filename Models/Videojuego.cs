using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("Videojuegos")]
    public class Videojuego
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Titulo")]
        [MaxLength(255)]
        public string? Titulo { get; set; }

        [Column("Descripcion")]
        public string? Descripcion { get; set; }

        [Column("Genero")]
        [MaxLength(100)]
        public string? Genero { get; set; }

        [Column("Plataforma")]
        [MaxLength(100)]
        public string? Plataforma { get; set; }

        [Column("Precio", TypeName = "decimal(10,2)")]
        public decimal? Precio { get; set; }

        [Column("imagen_url")]
        [MaxLength(500)]
        public string? ImagenUrl { get; set; }

        [Column("video_url")]
        [MaxLength(500)]
        public string? VideoUrl { get; set; }

        [Column("Stock")]
        public int? Stock { get; set; }

        [Column("PuntajePromedio", TypeName = "decimal(3,2)")]
        public double? PuntajePromedio { get; set; }

        [Column("TotalResenas")]
        public int TotalResenas { get; set; }

        [Column("Calidad")]
        [MaxLength(50)]
        public string? Calidad { get; set; }

        [Column("TotalVendidas")]
        public int TotalVendidas { get; set; }

        [Column("IngresosTotales", TypeName = "decimal(15,2)")]
        public decimal IngresosTotales { get; set; }

        [Column("EstadoStock")]
        [MaxLength(20)]
        public string? EstadoStock { get; set; }

        [Column("FechaLanzamiento")]
        public DateTime? FechaLanzamiento { get; set; }

        [Column("Desarrolladora")]
        [MaxLength(255)]
        public string? Desarrolladora { get; set; }

        [Column("FechaCreacion")]
        public DateTime? FechaCreacion { get; set; }
    }
}
