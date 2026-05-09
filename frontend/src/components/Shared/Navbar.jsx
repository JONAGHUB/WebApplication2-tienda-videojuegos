import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { authAPI, cartAPI } from '../../services/api';

const Navbar = () => {
  const [user, setUser] = useState(null);
  const [cartCount, setCartCount] = useState(0);
  const navigate = useNavigate();

  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
    
    updateCartCount();
  }, []);

  const updateCartCount = () => {
    cartAPI.getCart().then(({ data }) => {
      const total = data.reduce((sum, item) => sum + item.cantidad, 0);
      setCartCount(total);
    });
  };

  const handleLogout = async () => {
    try {
      await authAPI.logout();
      localStorage.removeItem('user');
      setUser(null);
      navigate('/');
    } catch (error) {
      console.error('Error al cerrar sesión:', error);
    }
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <Link to="/" className="navbar-logo">
          🎮 GameStore
        </Link>
        
        <ul className="navbar-menu">
          <li><Link to="/">Inicio</Link></li>
          <li><Link to="/games">Juegos</Link></li>
          
          {user ? (
            <>
              <li><Link to="/profile">Mi Perfil</Link></li>
              {user.rol === 'Admin' && (
                <li><Link to="/admin">Admin</Link></li>
              )}
              <li>
                <button onClick={handleLogout} className="btn-link">
                  Cerrar Sesión
                </button>
              </li>
            </>
          ) : (
            <>
              <li><Link to="/login">Iniciar Sesión</Link></li>
              <li><Link to="/register">Registrarse</Link></li>
            </>
          )}
          
          <li>
            <Link to="/cart" className="cart-link">
              🛒 Carrito {cartCount > 0 && <span className="badge">{cartCount}</span>}
            </Link>
          </li>
        </ul>
      </div>
    </nav>
  );
};

export default Navbar;