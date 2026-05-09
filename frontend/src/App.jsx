import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Layout from './components/Shared/Layout';
import HomePage from './components/Home/HomePage';
import GamesList from './components/Games/GamesList';
import GameDetail from './components/Games/GameDetail';
import AddGame from './components/Games/AddGame';
import CartPage from './components/Cart/CartPage';
import CheckoutPage from './components/Checkout/CheckoutPage';
import Login from './components/Account/Login';
import Register from './components/Account/Register';
import Profile from './components/Account/Profile';
import Dashboard from './components/Admin/Dashboard';
import ManageGames from './components/Admin/ManageGames';

// HOC para rutas protegidas
const ProtectedRoute = ({ children, adminOnly = false }) => {
  const user = JSON.parse(localStorage.getItem('user') || 'null');
  
  if (!user) {
    return <Navigate to="/login" replace />;
  }
  
  if (adminOnly && user.rol !== 'Admin') {
    return <Navigate to="/" replace />;
  }
  
  return children;
};

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<HomePage />} />
          <Route path="games" element={<GamesList />} />
          <Route path="games/:id" element={<GameDetail />} />
          <Route path="cart" element={<CartPage />} />
          <Route path="checkout" element={<CheckoutPage />} />
          <Route path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
          
          <Route path="profile" element={
            <ProtectedRoute>
              <Profile />
            </ProtectedRoute>
          } />
          
          <Route path="admin" element={
            <ProtectedRoute adminOnly>
              <Dashboard />
            </ProtectedRoute>
          } />
          
          <Route path="admin/games" element={
            <ProtectedRoute adminOnly>
              <ManageGames />
            </ProtectedRoute>
          } />
          
          <Route path="admin/games/add" element={
            <ProtectedRoute adminOnly>
              <AddGame />
            </ProtectedRoute>
          } />
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
