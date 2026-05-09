import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { cartAPI } from '../../services/api';

const CartPage = () => {
  const [cartItems, setCartItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    loadCart();
  }, []);

  const loadCart = async () => {
    try {
      setLoading(true);
      const { data } = await cartAPI.getCart();
      console.log('Carrito cargado:', data);
      setCartItems(Array.isArray(data) ? data : []);
    } catch (error) {
      console.error('Error al cargar carrito:', error);
      setCartItems([]);
    } finally {
      setLoading(false);
    }
  };

  const handleRemoveItem = async (videojuegoId) => {
    try {
      const { data } = await cartAPI.removeFromCart(videojuegoId);
      setCartItems(Array.isArray(data) ? data : []);
    } catch (error) {
      console.error('Error al eliminar item:', error);
    }
  };

  const handleUpdateQuantity = async (videojuegoId, newQuantity) => {
    if (newQuantity < 1) return;
    
    try {
      const { data } = await cartAPI.updateQuantity(videojuegoId, newQuantity);
      setCartItems(Array.isArray(data) ? data : []);
    } catch (error) {
      console.error('Error al actualizar cantidad:', error);
    }
  };

  const calculateTotal = () => {
    return cartItems.reduce((total, item) => {
      const precio = item.precio || item.Precio || 0;
      const cantidad = item.cantidad || item.Cantidad || 0;
      return total + (precio * cantidad);
    }, 0);
  };

  // Función para obtener la URL completa de la imagen
  const getImageUrl = (imagenUrl) => {
    if (!imagenUrl) return 'https://placehold.co/300x400/ccc/666?text=Sin+Imagen';
    
    if (imagenUrl.startsWith('http://') || imagenUrl.startsWith('https://')) {
      return imagenUrl;
    }
    
    const baseUrl = import.meta.env.VITE_API_URL?.replace('/api', '') || 'http://localhost:5001';
    return `${baseUrl}${imagenUrl}`;
  };

  if (loading) {
    return (
      <div className="cart-page">
        <h1>Carrito de Compras</h1>
        <p>Cargando...</p>
      </div>
    );
  }

  if (cartItems.length === 0) {
    return (
      <div className="cart-page empty-cart">
        <h1>Carrito de Compras</h1>
        <p>Tu carrito está vacío</p>
        <Link to="/games" className="btn btn-primary">
          Ir a la Tienda
        </Link>
      </div>
    );
  }

  return (
    <div className="cart-page">
      <h1>Carrito de Compras</h1>
      
      <div className="cart-container">
        <div className="cart-items">
          {cartItems.map((item) => {
            // Normalizar propiedades (soportar tanto PascalCase como camelCase)
            const videojuegoId = item.videojuegoId || item.VideojuegoId;
            const titulo = item.titulo || item.Titulo || 'Sin título';
            const imagenUrl = item.imagenUrl || item.ImagenUrl;
            const precio = item.precio || item.Precio || 0;
            const cantidad = item.cantidad || item.Cantidad || 0;
            const subtotal = precio * cantidad;

            return (
              <div key={videojuegoId} className="cart-item">
                <div className="item-image">
                  <img 
                    src={getImageUrl(imagenUrl)} 
                    alt={titulo}
                    onError={(e) => {
                      e.target.onerror = null;
                      e.target.src = 'https://placehold.co/300x400/ccc/666?text=Error+Imagen';
                    }}
                  />
                </div>
                <div className="item-details">
                  <h3>{titulo}</h3>
                  <p className="price">${precio.toFixed(2)}</p>
                </div>
                <div className="item-quantity">
                  <button 
                    onClick={() => handleUpdateQuantity(videojuegoId, cantidad - 1)}
                    className="btn-quantity"
                    disabled={cantidad <= 1}
                  >
                    -
                  </button>
                  <span className="quantity-display">{cantidad}</span>
                  <button 
                    onClick={() => handleUpdateQuantity(videojuegoId, cantidad + 1)}
                    className="btn-quantity"
                  >
                    +
                  </button>
                </div>
                <div className="item-total">
                  <strong>${subtotal.toFixed(2)}</strong>
                </div>
                <button
                  onClick={() => handleRemoveItem(videojuegoId)}
                  className="btn-remove"
                  title="Eliminar del carrito"
                >
                  ✕
                </button>
              </div>
            );
          })}
        </div>
        
        <div className="cart-summary">
          <h2>Resumen del Pedido</h2>
          <div className="summary-details">
            <div className="summary-row">
              <span>Productos ({cartItems.length}):</span>
              <span>${calculateTotal().toFixed(2)}</span>
            </div>
            <div className="summary-row">
              <span>Envío:</span>
              <span>Gratis</span>
            </div>
            <hr />
            <div className="summary-row total">
              <strong>Total:</strong>
              <strong>${calculateTotal().toFixed(2)}</strong>
            </div>
          </div>
          <button
            onClick={() => navigate('/checkout')}
            className="btn btn-primary btn-large"
          >
            Proceder al Pago
          </button>
          <Link to="/games" className="btn btn-secondary">
            Continuar Comprando
          </Link>
        </div>
      </div>
    </div>
  );
};

export default CartPage;