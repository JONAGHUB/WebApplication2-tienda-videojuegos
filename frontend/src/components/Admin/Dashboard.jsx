import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { adminAPI } from '../../services/api';

const Dashboard = () => {
  const [stats, setStats] = useState({
    totalGames: 0,
    totalUsers: 0,
    totalOrders: 0,
    totalRevenue: 0,
  });

  useEffect(() => {
    loadStats();
  }, []);

  const loadStats = async () => {
    try {
      const { data } = await adminAPI.getDashboardStats();
      setStats(data);
    } catch (error) {
      console.error('Error al cargar estadísticas:', error);
    }
  };

  return (
    <div className="admin-dashboard">
      <h1>Panel de Administración</h1>
      
      <div className="stats-grid">
        <div className="stat-card">
          <h3>Total Juegos</h3>
          <p className="stat-value">{stats.totalGames}</p>
        </div>
        <div className="stat-card">
          <h3>Total Usuarios</h3>
          <p className="stat-value">{stats.totalUsers}</p>
        </div>
        <div className="stat-card">
          <h3>Total Pedidos</h3>
          <p className="stat-value">{stats.totalOrders}</p>
        </div>
        <div className="stat-card">
          <h3>Ingresos Totales</h3>
          <p className="stat-value">${stats.totalRevenue?.toFixed(2)}</p>
        </div>
      </div>
      
      <div className="admin-actions">
        <Link to="/admin/games" className="btn btn-primary">
          Administrar Juegos
        </Link>
        <Link to="/admin/games/add" className="btn btn-secondary">
          Añadir Nuevo Juego
        </Link>
      </div>
    </div>
  );
};

export default Dashboard;