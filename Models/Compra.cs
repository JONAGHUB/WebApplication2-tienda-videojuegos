using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Compra
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int VideojuegoId { get; set; }
        public decimal PrecioCompra { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public string CodigoQr { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public Usuario? Usuario { get; set; }
        public Videojuego? Videojuego { get; set; }
    }
}
