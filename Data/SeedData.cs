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

            // Seed Usuarios (independiente de los videojuegos)
            if (!db.Usuarios.Any())
            {
                db.Usuarios.AddRange(
                    new Usuario
                    {
                        Nombre = "Administrador",
                        Correo = "admin@tienda.com",
                        Contrasena = "Admin123",
                        Rol = "Admin",
                        FechaCreacion = DateTime.UtcNow
                    },
                    new Usuario
                    {
                        Nombre = "Usuario Demo",
                        Correo = "usuario@tienda.com",
                        Contrasena = "Usuario123",
                        Rol = "Cliente",
                        FechaCreacion = DateTime.UtcNow
                    }
                );
                db.SaveChanges();
            }

            // Seed Videojuegos (solo si no existen)
            if (!db.Videojuegos.Any())
            {
                db.Videojuegos.AddRange(
                    new Videojuego
                    {
                        Titulo = "Eclipse: Dawn",
                        Descripcion = "RPG épico en mundos paralelos. Explora, combate y forja tu destino.",
                        Genero = "RPG",
                        Plataforma = "PC",
                        Precio = 59.99m,
                        ImagenUrl = "https://placehold.co/400x500/1a1a2e/eee?text=Eclipse+Dawn",
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
                        ImagenUrl = "https://placehold.co/400x500/e94560/fff?text=RacerX+Ultimate",
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
                        ImagenUrl = "https://placehold.co/400x500/16213e/0f3460?text=Mystic+Quest",
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
                        ImagenUrl = "https://placehold.co/400x500/533483/fff?text=Battlefront+Legends",
                        VideoUrl = "",
                        Stock = 200,
                        FechaLanzamiento = new DateTime(2025, 2, 28),
                        Desarrolladora = "Titan Forge",
                        FechaCreacion = DateTime.UtcNow
                    },
                    new Videojuego
                    {
                        Titulo = "Fantasy Realm",
                        Descripcion = "MMORPG masivo con clases únicas y batallas épicas.",
                        Genero = "RPG",
                        Plataforma = "PC",
                        Precio = 44.99m,
                        ImagenUrl = "https://placehold.co/400x500/9b59b6/fff?text=Fantasy+Realm",
                        VideoUrl = "",
                        Stock = 150,
                        FechaLanzamiento = new DateTime(2024, 5, 10),
                        Desarrolladora = "Epic Games Studio",
                        FechaCreacion = DateTime.UtcNow
                    },
                    new Videojuego
                    {
                        Titulo = "Cyber Ninja",
                        Descripcion = "Acción frenética en un mundo cyberpunk. Hackea, combate y sobrevive.",
                        Genero = "Acción",
                        Plataforma = "PC, Xbox",
                        Precio = 54.99m,
                        ImagenUrl = "https://placehold.co/400x500/00d2ff/000?text=Cyber+Ninja",
                        VideoUrl = "",
                        Stock = 95,
                        FechaLanzamiento = new DateTime(2024, 8, 22),
                        Desarrolladora = "Neon Studios",
                        FechaCreacion = DateTime.UtcNow
                    }
                );
                db.SaveChanges();
            }
        }
    }
}