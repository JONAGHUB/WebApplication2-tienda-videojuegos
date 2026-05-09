import React, { useState, useEffect } from 'react';
import { checkoutAPI } from '../../services/api';

const Profile = () => {
  const [user, setUser] = useState(null);
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      setUser(JSON.parse(storedUser));
      loadOrders();
    }
  }, []);

  const loadOrders = async () => {
    try {
      const { data } = await checkoutAPI.getOrders();
      setOrders(data);
    } catch (error) {
      console.error('Error al cargar órdenes:', error);
    } finally {
      setLoading(false);
    }
  };

  if (!user) return <div>Cargando...</div>;

  return (
    <div className="profile-page">
      <h1>Mi Perfil</h1>
      
      <div className="profile-container">
        <div className="profile-info">
          <h2>Información Personal</h2>
          <p><strong>Nombre:</strong> {user.nombre}</p>
          <p><strong>Email:</strong> {user.correo}</p>
          <p><strong>Rol:</strong> {user.rol}</p>
        </div>
        
        <div className="orders-section">
          <h2>Mis Pedidos</h2>
          {loading ? (
            <p>Cargando pedidos...</p>
          ) : orders.length === 0 ? (
            <p>No tienes pedidos aún</p>
          ) : (
            <div className="orders-list">
              {orders.map(order => (
                <div key={order.id} className="order-card">
                  <div className="order-header">
                    <span>Pedido #{order.id}</span>
                    <span>{new Date(order.fecha).toLocaleDateString()}</span>
                  </div>
                  <div className="order-body">
                    <p><strong>Total:</strong> ${order.total?.toFixed(2)}</p>
                    <p><strong>Estado:</strong> {order.estado || 'Procesando'}</p>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Profile;