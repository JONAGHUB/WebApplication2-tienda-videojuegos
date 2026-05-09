import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { cartAPI, checkoutAPI } from '../../services/api';

const CheckoutPage = () => {
  const navigate = useNavigate();
  const [cartItems, setCartItems] = useState([]);
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    nombre: '',
    direccion: '',
    ciudad: '',
    codigoPostal: '',
    telefono: '',
    metodoPago: 'tarjeta',
  });

  useEffect(() => {
    loadCart();
    checkAuth();
  }, []);

  const checkAuth = () => {
    const user = localStorage.getItem('user');
    if (!user) {
      alert('Debes iniciar sesión para realizar una compra');
      navigate('/login');
    }
  };

  const loadCart = async () => {
    try {
      const { data } = await cartAPI.getCart();
      if (data.length === 0) {
        alert('Tu carrito está vacío');
        navigate('/cart');
      }
      setCartItems(data);
    } catch (error) {
      console.error('Error al cargar carrito:', error);
    }
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const calculateTotal = () => {
    return cartItems.reduce((total, item) => total + (item.precio * item.cantidad), 0);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!formData.nombre || !formData.direccion || !formData.ciudad) {
      alert('Por favor completa todos los campos requeridos');
      return;
    }

    try {
      setLoading(true);
      const user = JSON.parse(localStorage.getItem('user'));
      
      const orderData = {
        usuarioId: user.id,
        items: cartItems.map(item => ({
          videojuegoId: item.videojuegoId,
          cantidad: item.cantidad,
          precioUnitario: item.precio,
        })),
        total: calculateTotal(),
        direccionEnvio: `${formData.direccion}, ${formData.ciudad}, ${formData.codigoPostal}`,
        metodoPago: formData.metodoPago,
      };

      await checkoutAPI.createOrder(orderData);
      await cartAPI.clearCart();
      
      alert('¡Compra realizada con éxito!');
      navigate('/profile');
    } catch (error) {
      console.error('Error al procesar compra:', error);
      alert('Error al procesar la compra. Intenta nuevamente.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="checkout-page">
      <h1>Finalizar Compra</h1>
      
      <div className="checkout-container">
        <form onSubmit={handleSubmit} className="checkout-form">
          <h2>Información de Envío</h2>
          
          <div className="form-group">
            <label>Nombre Completo *</label>
            <input
              type="text"
              name="nombre"
              value={formData.nombre}
              onChange={handleChange}
              required
            />
          </div>
          
          <div className="form-group">
            <label>Dirección *</label>
            <input
              type="text"
              name="direccion"
              value={formData.direccion}
              onChange={handleChange}
              required
            />
          </div>
          
          <div className="form-row">
            <div className="form-group">
              <label>Ciudad *</label>
              <input
                type="text"
                name="ciudad"
                value={formData.ciudad}
                onChange={handleChange}
                required
              />
            </div>
            
            <div className="form-group">
              <label>Código Postal</label>
              <input
                type="text"
                name="codigoPostal"
                value={formData.codigoPostal}
                onChange={handleChange}
              />
            </div>
          </div>
          
          <div className="form-group">
            <label>Teléfono</label>
            <input
              type="tel"
              name="telefono"
              value={formData.telefono}
              onChange={handleChange}
            />
          </div>
          
          <h2>Método de Pago</h2>
          <div className="form-group">
            <select
              name="metodoPago"
              value={formData.metodoPago}
              onChange={handleChange}
            >
              <option value="tarjeta">Tarjeta de Crédito/Débito</option>
              <option value="paypal">PayPal</option>
              <option value="transferencia">Transferencia Bancaria</option>
            </select>
          </div>
          
          <button
            type="submit"
            className="btn btn-primary btn-large"
            disabled={loading}
          >
            {loading ? 'Procesando...' : `Pagar $${calculateTotal().toFixed(2)}`}
          </button>
        </form>
        
        <div className="order-summary">
          <h2>Resumen del Pedido</h2>
          {cartItems.map(item => (
            <div key={item.videojuegoId} className="summary-item">
              <span>{item.titulo} x{item.cantidad}</span>
              <span>${(item.precio * item.cantidad).toFixed(2)}</span>
            </div>
          ))}
          <div className="summary-total">
            <strong>Total:</strong>
            <strong>${calculateTotal().toFixed(2)}</strong>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CheckoutPage;