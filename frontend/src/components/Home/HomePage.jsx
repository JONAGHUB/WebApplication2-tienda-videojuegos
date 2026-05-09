import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { gamesAPI } from '../../services/api';

const HomePage = () => {
  const [featuredGames, setFeaturedGames] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadFeaturedGames();
  }, []);

  const loadFeaturedGames = async () => {
    try {
      setLoading(true);
      setError(null);
      const { data } = await gamesAPI.getAll({ limit: 6 });
      
      // Validar que data sea un array y tenga elementos con id
      if (Array.isArray(data)) {
        const validGames = data.filter(game => game && game.id);
        setFeaturedGames(validGames);
      } else {
        console.warn('La respuesta de la API no es un array:', data);
        setFeaturedGames([]);
      }
    } catch (error) {
      console.error('Error al cargar juegos destacados:', error);
      setError('No se pudieron cargar los juegos destacados');
      setFeaturedGames([]);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="loading">
        <p>Cargando...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="error">
        <p>{error}</p>
        <button onClick={loadFeaturedGames}>Reintentar</button>
      </div>
    );
  }

  return (
    <div className="home-page">
      <section className="hero">
        <h1>Bienvenido a GameStore</h1>
        <p>Encuentra los mejores videojuegos al mejor precio</p>
        <Link to="/games" className="btn btn-primary">Ver Catálogo</Link>
      </section>

      <section className="featured-games">
        <h2>Juegos Destacados</h2>
        {featuredGames.length === 0 ? (
          <p className="no-games">No hay juegos disponibles en este momento.</p>
        ) : (
          <div className="games-grid">
            {featuredGames.map((game) => (
              <div key={game.id} className="game-card">
                <img 
                  src={game.imagenUrl || '/images/placeholder.png'} 
                  alt={game.titulo || 'Videojuego'} 
                  onError={(e) => {
                    e.target.src = '/images/placeholder.png';
                  }}
                />
                <h3>{game.titulo || 'Sin título'}</h3>
                <p className="price">
                  ${game.precio ? game.precio.toFixed(2) : '0.00'}
                </p>
                <Link to={`/games/${game.id}`} className="btn btn-secondary">
                  Ver Detalles
                </Link>
              </div>
            ))}
          </div>
        )}
      </section>
    </div>
  );
};

export default HomePage;