import React from 'react';
import { Button as BootstrapButton } from 'react-bootstrap';

interface CommonButtonProps {
  children: React.ReactNode;
  onClick?: () => void;
  variant?: string;
  size?: 'sm' | 'lg';
  disabled?: boolean;
  className?: string;
  style?: React.CSSProperties;
  type?: 'button' | 'submit' | 'reset';
}

const CommonButton: React.FC<CommonButtonProps> = ({ 
  children, 
  onClick, 
  variant = 'danger',
  size,
  disabled = false,
  className = '',
  style = {},
  type = 'button'
}) => {
  const defaultStyle = {
    backgroundColor: '#986277',
    borderColor: '#986277',
    color: '#e5bbb1',
    fontWeight: 'bold',
    transition: 'all 0.3s ease',
    ...style
  };

  return (
    <BootstrapButton
      variant={variant}
      size={size}
      onClick={onClick}
      disabled={disabled}
      className={className}
      style={defaultStyle}
      type={type}
    >
      {children}
    </BootstrapButton>
  );
};

export default CommonButton; 