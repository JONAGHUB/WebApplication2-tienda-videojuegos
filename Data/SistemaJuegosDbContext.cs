using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class SistemaJuegosDbContext : DbContext
    {
        public SistemaJuegosDbContext(DbContextOptions<SistemaJuegosDbContext> options)
            : base(options)
        {
        }

        // Entidades principales
        public DbSet<Videojuego> Videojuegos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetallesCompra { get; set; }

        // Nuevas entidades
        public DbSet<Resena> Resenas { get; set; }            // mapeo en OnModelCreating a tabla "Reseñas"
        public DbSet<CostoInventario> CostosInventario { get; set; }
        public DbSet<ReporteVenta> ReportesVentas { get; set; }
        public DbSet<CodigoQR> CodigosQR { get; set; }
        public DbSet<Video> Videos { get; set; }

        // Alias para compatibilidad con código que usa el identificador 'Reseñas' (con tilde)
        // No mapeada por EF Core para evitar duplicidad en el modelo.
        [NotMapped]
        public DbSet<Resena> Reseñas => Resenas;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapear la entidad Resena a la tabla que existe en la BDD llamada "Reseñas"
            modelBuilder.Entity<Resena>(entity =>
            {
                entity.ToTable("Reseñas"); // Nombre exacto de la tabla en la base de datos

                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.Videojuego)
                      .WithMany()
                      .HasForeignKey(r => r.VideojuegoId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Usuario)
                      .WithMany()
                      .HasForeignKey(r => r.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CostoInventario>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.Videojuego)
                      .WithMany()
                      .HasForeignKey(c => c.VideojuegoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CodigoQR>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.HasOne(q => q.Videojuego)
                      .WithMany()
                      .HasForeignKey(q => q.VideojuegoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DetalleCompra>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.HasOne(d => d.Compra)
                      .WithMany()
                      .HasForeignKey(d => d.CompraId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Videojuego)
                      .WithMany()
                      .HasForeignKey(d => d.VideojuegoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Video>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.HasOne(v => v.Videojuego)
                      .WithMany()
                      .HasForeignKey(v => v.VideojuegoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
