using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("Resenas")]
    public class Resena
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("VideojuegoId")]
        public int VideojuegoId { get; set; }

        [Column("UsuarioId")]
        public int UsuarioId { get; set; }

        [Column("Puntuacion")]
        [Range(1, 5)]
        public int Puntuacion { get; set; }

        [Column("Comentario")]
        [MaxLength(1000)]
        public string? Comentario { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        // Navegación
        [ForeignKey("VideojuegoId")]
        public virtual Videojuego? Videojuego { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}