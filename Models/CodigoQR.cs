using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    [Table("codigos_qr")]
    public class CodigoQR
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("videojuego_id")]
        public int VideojuegoId { get; set; }

        [Column("codigo_generado")]
        public string? CodigoGenerado { get; set; }

        [Column("url_qr_imagen")]
        public string? UrlQrImagen { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("veces_escaneado")]
        public int VecesEscaneado { get; set; }

        // Navegación
        [ForeignKey("VideojuegoId")]
        public virtual Videojuego? Videojuego { get; set; }
    }
}