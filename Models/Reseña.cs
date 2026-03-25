using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Reseña
    {
        [Key]
        public int Id { get; set; }
        public int VideojuegoId { get; set; }
        public int UsuarioId { get; set; }
        public int Calificacion { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public Videojuego? Videojuego { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
