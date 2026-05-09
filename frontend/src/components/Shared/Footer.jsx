import React from 'react';

const Footer = () => {
  return (
    <footer className="footer">
      <div className="footer-container">
        <p>&copy; {new Date().getFullYear()} GameStore. Todos los derechos reservados.</p>
        <div className="footer-links">
          <a href="#terms">Términos</a>
          <a href="#privacy">Privacidad</a>
          <a href="#contact">Contacto</a>
        </div>
      </div>
    </footer>
  );
};

export default Footer;