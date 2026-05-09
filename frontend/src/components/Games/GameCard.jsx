import React from 'react';
import { Link } from 'react-router-dom';

const GameCard = ({ game, onAddToCart }) => {
  return (
    <div className="game-card">
      <div className="game-image">
        <img 
          src={game.imagenUrl || 'https://placehold.co/400x500/ccc/666?text=Sin+Imagen'} 
          alt={game.titulo || 'Videojuego'} 
          onError={(e) => {
            e.target.onerror = null;
            e.target.src = 'https://placehold.co/400x500/ccc/666?text=Error+Imagen';
          }}
        />
        {(!game.stock || game.stock === 0) && (
          <div className="out-of-stock-badge">Agotado</div>
        )}
      </div>
      
      <div className="game-info">
        <h3>{game.titulo || 'Sin título'}</h3>
        <p className="developer">{game.desarrolladora || 'Desarrolladora desconocida'}</p>
        <p className="genre">
          {game.genero || 'N/A'} • {game.plataforma || 'N/A'}
        </p>
        <p className="price">
          ${game.precio ? game.precio.toFixed(2) : '0.00'}
        </p>
        
        <div className="rating">
          ⭐ {game.puntajePromedio ? game.puntajePromedio.toFixed(1) : 'N/A'}
        </div>
        
        <div className="game-actions">
          <Link to={`/games/${game.id}`} className="btn btn-secondary">
            Ver Detalles
          </Link>
          {onAddToCart && (
            <button
              onClick={() => onAddToCart(game.id)}
              className="btn btn-primary"
              disabled={!game.stock || game.stock === 0}
            >
              {!game.stock || game.stock === 0 ? 'Agotado' : 'Añadir al Carrito'}
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default GameCard;

// En el componente donde estás usando GameCard para mostrar los juegos destacados
import GameCard from '../Games/GameCard';

// Dentro del map:
{featuredGames.map((game) => (
  <GameCard key={game.id} game={game} onAddToCart={handleAddToCart} />
))}