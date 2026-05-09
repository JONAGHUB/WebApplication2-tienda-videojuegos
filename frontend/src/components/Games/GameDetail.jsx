import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { gamesAPI, cartAPI } from '../../services/api';

const GameDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [game, setGame] = useState(null);
  const [loading, setLoading] = useState(true);
  const [cantidad, setCantidad] = useState(1);

  useEffect(() => {
    loadGameDetail();
  }, [id]);

  const loadGameDetail = async () => {
    try {
      const { data } = await gamesAPI.getById(id);
      setGame(data);
    } catch (error) {
      console.error('Error al cargar detalles del juego:', error);
      alert('Juego no encontrado');
      navigate('/games');
    } finally {
      setLoading(false);
    }
  };

  const handleAddToCart = async () => {
    try {
      await cartAPI.addToCart(game.id, cantidad);
      alert(`${cantidad} unidad(es) añadida(s) al carrito`);
      navigate('/cart');
    } catch (error) {
      console.error('Error al añadir al carrito:', error);
      alert('Error al añadir al carrito');
    }
  };

  if (loading) return <div className="loading">Cargando...</div>;
  if (!game) return <div>Juego no encontrado</div>;

  return (
    <div className="game-detail-page">
      <div className="game-detail-container">
        <div className="game-image">
          <img src={game.imagenUrl || '/images/placeholder.png'} alt={game.titulo} />
        </div>
        
        <div className="game-details">
          <h1>{game.titulo}</h1>
          <div className="game-meta">
            <span className="genre">{game.genero}</span>
            <span className="platform">{game.plataforma}</span>
            <span className="rating">⭐ {game.puntajePromedio?.toFixed(1) || 'N/A'}</span>
          </div>
          
          <p className="description">{game.descripcion}</p>
          
          <div className="game-info-grid">
            <div className="info-item">
              <strong>Desarrolladora:</strong>
              <span>{game.desarrolladora || 'N/A'}</span>
            </div>
            <div className="info-item">
              <strong>Fecha de Lanzamiento:</strong>
              <span>{game.fechaLanzamiento ? new Date(game.fechaLanzamiento).toLocaleDateString() : 'N/A'}</span>
            </div>
            <div className="info-item">
              <strong>Stock:</strong>
              <span>{game.stock > 0 ? `${game.stock} disponibles` : 'Agotado'}</span>
            </div>
          </div>
          
          <div className="price-section">
            <h2 className="price">${game.precio?.toFixed(2)}</h2>
          </div>
          
          {game.stock > 0 && (
            <div className="purchase-section">
              <div className="quantity-selector">
                <label>Cantidad:</label>
                <input
                  type="number"
                  min="1"
                  max={game.stock}
                  value={cantidad}
                  onChange={(e) => setCantidad(Math.max(1, parseInt(e.target.value) || 1))}
                />
              </div>
              <button onClick={handleAddToCart} className="btn btn-primary btn-large">
                Añadir al Carrito
              </button>
            </div>
          )}
          
          {game.videoUrl && (
            <div className="video-section">
              <h3>Video</h3>
              <iframe
                src={game.videoUrl}
                title="Game Trailer"
                allowFullScreen
              />
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default GameDetail;