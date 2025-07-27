import React from 'react';
import { Card as BootstrapCard } from 'react-bootstrap';

interface CommonCardProps {
  children: React.ReactNode;
  className?: string;
  style?: React.CSSProperties;
}

const CommonCard: React.FC<CommonCardProps> = ({ children, className = '', style = {} }) => {
  const defaultStyle = {
    backgroundColor: 'rgba(229, 187, 177, 0.95)',
    borderColor: '#986277',
    boxShadow: '0 4px 15px rgba(76, 57, 73, 0.2)',
    ...style
  };

  return (
    <BootstrapCard 
      className={`shadow ${className}`}
      style={defaultStyle}
    >
      {children}
    </BootstrapCard>
  );
};

export default CommonCard; 