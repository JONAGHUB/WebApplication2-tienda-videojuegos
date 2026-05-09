import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { gamesAPI, cartAPI } from '../../services/api';

const GamesList = () => {
    const [games, setGames] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [searchTerm, setSearchTerm] = useState('');
    const [filters, setFilters] = useState({
        genero: '',
        plataforma: '',
    });

    useEffect(() => {
        loadGames();
    }, [filters]);

    const loadGames = async () => {
        try {
            setLoading(true);
            setError(null);
            const { data } = await gamesAPI.getAll(filters);

            console.log('Datos recibidos del backend:', data);

            if (Array.isArray(data)) {
                const validGames = data.filter(game => {
                    // Validamos que tenga ID y Título (basado en las mayúsculas de tu consola)
                    if (!game.Id) return false;
                    if (!game.Titulo || game.Titulo.trim() === '') return false;

                    return true;
                });

                console.log(`${validGames.length} juegos válidos de ${data.length} totales`);
                setGames(validGames);
            } else {
                setGames([]);
            }
        } catch (error) {
            console.error('Error al cargar juegos:', error);
            setError('No se pudieron cargar los juegos');
            setGames([]);
        } finally {
            setLoading(false);
        }
    };

    const handleSearch = async (e) => {
        e.preventDefault();
        if (!searchTerm.trim()) {
            loadGames();
            return;
        }

        try {
            const { data } = await gamesAPI.search(searchTerm);
            const validGames = Array.isArray(data)
                ? data.filter(g => g.Id && g.Titulo)
                : [];
            setGames(validGames);
        } catch (error) {
            console.error('Error en búsqueda:', error);
            setError('Error en la búsqueda');
        }
    };

    const handleAddToCart = async (gameId) => {
        try {
            await cartAPI.addToCart(gameId);
            alert('Juego añadido al carrito');
        } catch (error) {
            console.error('Error al añadir al carrito:', error);
            alert('Error al añadir al carrito');
        }
    };

    // --- FUNCIÓN CORREGIDA ---
    const getImageUrl = (game) => {
        // Intentamos obtener la imagen de cualquiera de las dos propiedades posibles
        const path = game.ImagenUrl || game.imagen_url;

        if (!path) return 'https://placehold.co/400x500/ccc/666?text=Sin+Imagen';

        if (path.startsWith('http://') || path.startsWith('https://')) {
            return path;
        }

        // Definimos el puerto 5001 como base para el backend
        const baseUrl = 'http://localhost:5001';

        // Nos aseguramos de que haya una sola barra entre la base y la ruta
        const cleanPath = path.startsWith('/') ? path : `/${path}`;
        return `${baseUrl}${cleanPath}`;
    };

    if (loading) return <div className="loading">Cargando juegos...</div>;

    if (error) {
        return (
            <div className="error">
                <p>{error}</p>
                <button onClick={loadGames} className="btn btn-primary">Reintentar</button>
            </div>
        );
    }

    return (
        <div className="games-list-page">
            <h1>Catálogo de Juegos</h1>

            <div className="search-section">
                <form onSubmit={handleSearch} className="search-form">
                    <input
                        type="text"
                        placeholder="Buscar juegos..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                    />
                    <button type="submit" className="btn btn-primary">Buscar</button>
                </form>

                <div className="filters">
                    <select
                        value={filters.genero}
                        onChange={(e) => setFilters({ ...filters, genero: e.target.value })}
                    >
                        <option value="">Todos los géneros</option>
                        <option value="Acción">Acción</option>
                        <option value="Aventura">Aventura</option>
                        <option value="RPG">RPG</option>
                        <option value="Deportes">Deportes</option>
                        <option value="Shooter">Shooter</option>
                    </select>

                    <select
                        value={filters.plataforma}
                        onChange={(e) => setFilters({ ...filters, plataforma: e.target.value })}
                    >
                        <option value="">Todas las plataformas</option>
                        <option value="PC">PC</option>
                        <option value="PlayStation">PlayStation</option>
                        <option value="Xbox">Xbox</option>
                        <option value="Nintendo Switch">Nintendo Switch</option>
                    </select>
                </div>
            </div>

            <div className="games-grid">
                {games.length === 0 ? (
                    <p className="no-games">No se encontraron juegos disponibles</p>
                ) : (
                    games.map((game) => (
                        <div key={game.Id} className="game-card">
                            <div className="game-image">
                                <img
                                    src={getImageUrl(game)} // Pasamos el objeto completo 'game'
                                    alt={game.Titulo}
                                    onError={(e) => {
                                        e.target.onerror = null;
                                        e.target.src = 'https://placehold.co/400x500/ccc/666?text=Error+Imagen';
                                    }}
                                />
                                {(!game.Stock || game.Stock === 0) && (
                                    <div className="out-of-stock-badge">Agotado</div>
                                )}
                            </div>
                            <div className="game-info">
                                <h3>{game.Titulo}</h3>
                                <p className="developer">{game.Desarrolladora || 'Desarrolladora desconocida'}</p>
                                <p className="genre">
                                    {game.Genero || 'Sin género'} • {game.Plataforma || 'Sin plataforma'}
                                </p>
                                <p className="price">
                                    ${game.Precio ? game.Precio.toFixed(2) : '0.00'}
                                </p>
                                <div className="rating">
                                    ⭐ {game.PuntajePromedio ? game.PuntajePromedio.toFixed(1) : 'N/A'}
                                </div>
                                <div className="game-actions">
                                    <Link to={`/games/${game.Id}`} className="btn btn-secondary">
                                        Ver Detalles
                                    </Link>
                                    <button
                                        onClick={() => handleAddToCart(game.Id)}
                                        className="btn btn-primary"
                                        disabled={!game.Stock || game.Stock === 0}
                                    >
                                        {!game.Stock || game.Stock === 0 ? 'Agotado' : 'Añadir al Carrito'}
                                    </button>
                                </div>
                            </div>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
};

export default GamesList;