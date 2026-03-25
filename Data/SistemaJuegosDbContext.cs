using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class SistemaJuegosDbContext : DbContext
    {
        public SistemaJuegosDbContext(DbContextOptions<SistemaJuegosDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Videojuego> Videojuegos { get; set; }
        public DbSet<Reseña> Reseñas { get; set; }
        public DbSet<Compra> Compras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Videojuego>().ToTable("Videojuegos");
            modelBuilder.Entity<Reseña>().ToTable("Reseñas");
        }
    }
}
