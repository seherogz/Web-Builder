import React from 'react';
import { Card } from 'react-bootstrap';

interface HeaderProps {
  title: string;
  subtitle?: string;
}

const Header: React.FC<HeaderProps> = ({ title, subtitle }) => {
  return (
    <Card.Header 
      className="bg-dark text-white" 
      style={{backgroundColor: '#4c3949 !important'}}
    >
      <h1 className="text-center mb-0">
        <i className="fas fa-hotel me-2"></i>
        {title}
      </h1>
      {subtitle && (
        <p className="text-center mb-0 mt-2" style={{color: '#e5bbb1', fontSize: '0.9rem'}}>
          {subtitle}
        </p>
      )}
    </Card.Header>
  );
};

export default Header; 