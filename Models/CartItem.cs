namespace WebApplication2.Models
{
    public class CartItem
    {
        public int VideojuegoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; } = 1;
    }
}