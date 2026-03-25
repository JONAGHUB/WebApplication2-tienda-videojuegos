using System;

namespace WebApplication2.Models
{
    public class ReviewViewModel
    {
        public int Calificacion { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string UsuarioNombre { get; set; } = "Anónimo";
    }
}