import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { gamesAPI } from '../../services/api';

const AddGame = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    titulo: '',
    descripcion: '',
    genero: '',
    plataforma: '',
    precio: '',
    imagenUrl: '',
    videoUrl: '',
    stock: '',
    desarrolladora: '',
    fechaLanzamiento: '',
  });
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const gameData = {
        ...formData,
        precio: parseFloat(formData.precio),
        stock: parseInt(formData.stock),
      };
      
      await gamesAPI.create(gameData);
      alert('Juego creado exitosamente');
      navigate('/admin/games');
    } catch (error) {
      console.error('Error al crear juego:', error);
      alert('Error al crear el juego');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="add-game-page">
      <h1>Añadir Nuevo Juego</h1>
      
      <form onSubmit={handleSubmit} className="game-form">
        <div className="form-group">
          <label>Título *</label>
          <input
            type="text"
            name="titulo"
            value={formData.titulo}
            onChange={handleChange}
            required
          />
        </div>
        
        <div className="form-group">
          <label>Descripción</label>
          <textarea
            name="descripcion"
            value={formData.descripcion}
            onChange={handleChange}
            rows={4}
          />
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>Género</label>
            <input
              type="text"
              name="genero"
              value={formData.genero}
              onChange={handleChange}
            />
          </div>
          
          <div className="form-group">
            <label>Plataforma</label>
            <input
              type="text"
              name="plataforma"
              value={formData.plataforma}
              onChange={handleChange}
            />
          </div>
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>Precio *</label>
            <input
              type="number"
              name="precio"
              value={formData.precio}
              onChange={handleChange}
              step="0.01"
              required
            />
          </div>
          
          <div className="form-group">
            <label>Stock *</label>
            <input
              type="number"
              name="stock"
              value={formData.stock}
              onChange={handleChange}
              required
            />
          </div>
        </div>
        
        <div className="form-group">
          <label>URL de Imagen</label>
          <input
            type="url"
            name="imagenUrl"
            value={formData.imagenUrl}
            onChange={handleChange}
          />
        </div>
        
        <div className="form-group">
          <label>URL de Video</label>
          <input
            type="url"
            name="videoUrl"
            value={formData.videoUrl}
            onChange={handleChange}
          />
        </div>
        
        <div className="form-group">
          <label>Desarrolladora</label>
          <input
            type="text"
            name="desarrolladora"
            value={formData.desarrolladora}
            onChange={handleChange}
          />
        </div>
        
        <div className="form-group">
          <label>Fecha de Lanzamiento</label>
          <input
            type="date"
            name="fechaLanzamiento"
            value={formData.fechaLanzamiento}
            onChange={handleChange}
          />
        </div>
        
        <div className="form-actions">
          <button
            type="submit"
            className="btn btn-primary"
            disabled={loading}
          >
            {loading ? 'Guardando...' : 'Guardar Juego'}
          </button>
          <button
            type="button"
            onClick={() => navigate('/admin/games')}
            className="btn btn-secondary"
          >
            Cancelar
          </button>
        </div>
      </form>
    </div>
  );
};

export default AddGame;