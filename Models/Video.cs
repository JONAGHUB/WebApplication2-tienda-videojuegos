using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("videos")]
    public class Video
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("VideojuegoId")]
        public int VideojuegoId { get; set; }

        [Column("Titulo")]
        public string? Titulo { get; set; }

        [Column("UrlYoutube")]
        public string UrlYoutube { get; set; } = string.Empty;

        // Guardamos el tipo como texto para compatibilidad con el enum SQL
        [Column("Tipo")]
        public string? Tipo { get; set; } // "trailer", "gameplay", "review"

        [Column("Orden")]
        public int Orden { get; set; } = 1;

        [Column("Activo")]
        public bool Activo { get; set; } = true;

        [Column("FechaAgregado")]
        public DateTime FechaAgregado { get; set; } = DateTime.UtcNow;

        // Navegación
        public Videojuego? Videojuego { get; set; }
    }
}