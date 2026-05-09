import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5001/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true,
});

// Interceptor para añadir token si existe
api.interceptors.request.use(
  (config) => {
    const user = JSON.parse(localStorage.getItem('user') || 'null');
    if (user?.token) {
      config.headers.Authorization = `Bearer ${user.token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Interceptor para manejar errores globalmente
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// === AUTENTICACIÓN ===
export const authAPI = {
  login: (correo, contrasena) => 
    api.post('/auth/login', { correo, contrasena }),
  
  register: (nombre, correo, contrasena) => 
    api.post('/auth/register', { nombre, correo, contrasena }),
  
  logout: () => api.post('/auth/logout'),
  
  getCurrentUser: () => api.get('/auth/me'),
};

// === VIDEOJUEGOS ===
export const gamesAPI = {
  getAll: (params = {}) => 
    api.get('/videojuegosapi', { params }),
  
  getById: (id) => 
    api.get(`/videojuegosapi/${id}`),
  
  create: (gameData) => 
    api.post('/videojuegosapi', gameData),
  
  update: (id, gameData) => 
    api.put(`/videojuegosapi/${id}`, gameData),
  
  delete: (id) => 
    api.delete(`/videojuegosapi/${id}`),
  
  search: (query) => 
    api.get('/videojuegosapi/search', { params: { q: query } }),
};

// === CARRITO ===
export const cartAPI = {
  getCart: () => {
    const cart = localStorage.getItem('cart');
    return Promise.resolve({ data: cart ? JSON.parse(cart) : [] });
  },
  
  addToCart: (videojuegoId, cantidad = 1) => {
    return gamesAPI.getById(videojuegoId).then(({ data: juego }) => {
      console.log('Juego recibido para agregar al carrito:', juego);
      
      const cart = JSON.parse(localStorage.getItem('cart') || '[]');
      
      // Normalizar ID (puede venir como Id o id)
      const gameId = juego.Id || juego.id;
      const existing = cart.find(item => 
        (item.videojuegoId === gameId) || (item.videojuegoId === videojuegoId)
      );
      
      if (existing) {
        existing.cantidad += cantidad;
        console.log('Cantidad actualizada:', existing);
      } else {
        // Crear item con propiedades normalizadas
        const newItem = {
          videojuegoId: gameId,
          titulo: juego.Titulo || juego.titulo || 'Sin título',
          precio: juego.Precio || juego.precio || 0,
          cantidad: cantidad,
          imagenUrl: juego.ImagenUrl || juego.imagenUrl || '',
        };
        
        console.log('Nuevo item agregado al carrito:', newItem);
        cart.push(newItem);
      }
      
      localStorage.setItem('cart', JSON.stringify(cart));
      console.log('Carrito actualizado:', cart);
      return { data: cart };
    });
  },
  
  removeFromCart: (videojuegoId) => {
    const cart = JSON.parse(localStorage.getItem('cart') || '[]');
    const updated = cart.filter(item => item.videojuegoId !== videojuegoId);
    localStorage.setItem('cart', JSON.stringify(updated));
    console.log('Item eliminado. Carrito actualizado:', updated);
    return Promise.resolve({ data: updated });
  },
  
  updateQuantity: (videojuegoId, cantidad) => {
    const cart = JSON.parse(localStorage.getItem('cart') || '[]');
    const item = cart.find(i => i.videojuegoId === videojuegoId);
    if (item) {
      item.cantidad = cantidad;
      localStorage.setItem('cart', JSON.stringify(cart));
      console.log('Cantidad actualizada:', item);
    }
    return Promise.resolve({ data: cart });
  },
  
  clearCart: () => {
    localStorage.removeItem('cart');
    console.log('Carrito vaciado');
    return Promise.resolve({ data: [] });
  },
};

// === CHECKOUT / COMPRAS ===
export const checkoutAPI = {
  createOrder: (orderData) => 
    api.post('/compras', orderData),
  
  getOrders: () => 
    api.get('/compras'),
  
  getOrderById: (id) => 
    api.get(`/compras/${id}`),
};

// === ADMIN ===
export const adminAPI = {
  getDashboardStats: () => 
    api.get('/admin/stats'),
  
  getAllUsers: () => 
    api.get('/admin/usuarios'),
  
  getAllOrders: () => 
    api.get('/admin/compras'),
};

export default api;