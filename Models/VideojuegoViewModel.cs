using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class VideojuegoViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Plataforma { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int Stock { get; set; }
        public double PuntajePromedio { get; set; }
    }
}
