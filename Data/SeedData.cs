using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public static class SeedData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SistemaJuegosDbContext>();

            if (db.Videojuegos.Any()) return;

            db.Videojuegos.AddRange(
                new Videojuego
                {
                    Titulo = "Eclipse: Dawn",
                    Descripcion = "RPG épico en mundos paralelos. Explora, combate y forja tu destino.",
                    Genero = "RPG",
                    Plataforma = "PC",
                    Precio = 59.99m,
                    ImagenUrl = "/images/eclipse_dawn.jpg",
                    VideoUrl = "",
                    Stock = 120,
                    FechaLanzamiento = new DateTime(2024, 11, 15),
                    Desarrolladora = "Nebula Studio",
                    FechaCreacion = DateTime.UtcNow
                },
                new Videojuego
                {
                    Titulo = "RacerX Ultimate",
                    Descripcion = "Velocidad extrema y circuitos futuristas. Modifica tu nave y domina la pista.",
                    Genero = "Racing",
                    Plataforma = "PC, PS5",
                    Precio = 39.99m,
                    ImagenUrl = "/images/racerx_ultimate.jpg",
                    VideoUrl = "",
                    Stock = 80,
                    FechaLanzamiento = new DateTime(2023, 6, 3),
                    Desarrolladora = "Velocity Works",
                    FechaCreacion = DateTime.UtcNow
                },
                new Videojuego
                {
                    Titulo = "Mystic Quest",
                    Descripcion = "Aventura de puzles y exploración con una historia emotiva.",
                    Genero = "Adventure",
                    Plataforma = "PS5",
                    Precio = 29.99m,
                    ImagenUrl = "/images/mystic_quest.jpg",
                    VideoUrl = "",
                    Stock = 50,
                    FechaLanzamiento = new DateTime(2022, 9, 20),
                    Desarrolladora = "Oak & Ember",
                    FechaCreacion = DateTime.UtcNow
                },
                new Videojuego
                {
                    Titulo = "Battlefront Legends",
                    Descripcion = "Shooter táctico por equipos con mapas enormes y progresión competitiva.",
                    Genero = "Shooter",
                    Plataforma = "PC, PS5",
                    Precio = 49.99m,
                    ImagenUrl = "/images/battlefront_legends.jpg",
                    VideoUrl = "",
                    Stock = 200,
                    FechaLanzamiento = new DateTime(2025, 2, 28),
                    Desarrolladora = "Titan Forge",
                    FechaCreacion = DateTime.UtcNow
                }
            );

            db.SaveChanges();
        }
    }
}