import axios from 'axios';

// Crear instancia de axios con configuración base
const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5001',
  withCredentials: true, // Importante para cookies de autenticación
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000, // 10 segundos
});

// Interceptor de solicitudes (opcional - para agregar tokens, etc.)
api.interceptors.request.use(
  (config) => {
    // Aquí puedes agregar tokens si los usas en el futuro
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Interceptor de respuestas (opcional - para manejo global de errores)
api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response?.status === 401) {
      // Redirigir a login si no está autenticado
      console.log('No autenticado');
    }
    return Promise.reject(error);
  }
);

export default api;