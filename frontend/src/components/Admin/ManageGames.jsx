import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { gamesAPI } from '../../services/api';

const ManageGames = () => {
  const [games, setGames] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadGames();
  }, []);

  const loadGames = async () => {
    try {
      const { data } = await gamesAPI.getAll();
      setGames(data);
    } catch (error) {
      console.error('Error al cargar juegos:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('¿Estás seguro de eliminar este juego?')) return;
    
    try {
      await gamesAPI.delete(id);
      setGames(games.filter(g => g.id !== id));
      alert('Juego eliminado exitosamente');
    } catch (error) {
      console.error('Error al eliminar juego:', error);
      alert('Error al eliminar el juego');
    }
  };

  if (loading) return <div className="loading">Cargando...</div>;

  return (
    <div className="manage-games-page">
      <div className="page-header">
        <h1>Administrar Juegos</h1>
        <Link to="/admin/games/add" className="btn btn-primary">
          + Añadir Juego
        </Link>
      </div>
      
      <div className="games-table">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Título</th>
              <th>Género</th>
              <th>Plataforma</th>
              <th>Precio</th>
              <th>Stock</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {games.map(game => (
              <tr key={game.id}>
                <td>{game.id}</td>
                <td>{game.titulo}</td>
                <td>{game.genero}</td>
                <td>{game.plataforma}</td>
                <td>${game.precio?.toFixed(2)}</td>
                <td>{game.stock}</td>
                <td>
                  <button className="btn btn-sm btn-secondary">
                    Editar
                  </button>
                  <button
                    onClick={() => handleDelete(game.id)}
                    className="btn btn-sm btn-danger"
                  >
                    Eliminar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ManageGames;