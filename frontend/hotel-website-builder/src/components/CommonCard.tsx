import React from 'react';
import { Card as BootstrapCard } from 'react-bootstrap';

interface CommonCardProps {
  children: React.ReactNode;
  className?: string;
  style?: React.CSSProperties;
  title?: string;
}

const CommonCard: React.FC<CommonCardProps> = ({ children, className = '', style = {}, title }) => {
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
      {title && (
        <BootstrapCard.Header style={{ backgroundColor: '#986277', color: 'white' }}>
          <h5 className="mb-0">{title}</h5>
        </BootstrapCard.Header>
      )}
      <BootstrapCard.Body>
        {children}
      </BootstrapCard.Body>
    </BootstrapCard>
  );
};

export default CommonCard; 