using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("videojuegos")]
    public class Videojuego
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("titulo")]
        public string? Titulo { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("genero")]
        public string? Genero { get; set; }

        [Column("plataforma")]
        public string? Plataforma { get; set; }

        [Column("precio")]
        public decimal? Precio { get; set; }

        [Column("imagen_url")]
        public string? ImagenUrl { get; set; }

        [Column("video_url")]
        public string? VideoUrl { get; set; }

        [Column("stock")]
        public int? Stock { get; set; }

        [Column("puntaje_promedio")]
        public double? PuntajePromedio { get; set; }

        [Column("fecha_lanzamiento")]
        public DateTime? FechaLanzamiento { get; set; }

        [Column("desarrolladora")]
        public string? Desarrolladora { get; set; }

        [Column("fecha_creacion")]
        public DateTime? FechaCreacion { get; set; }
    }
}
