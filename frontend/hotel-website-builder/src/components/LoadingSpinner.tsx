import React from 'react';
import { ProgressBar } from 'react-bootstrap';

interface LoadingSpinnerProps {
  message?: string;
  progress?: number;
}

const LoadingSpinner: React.FC<LoadingSpinnerProps> = ({ 
  message = "Yükleniyor...", 
  progress = 100 
}) => {
  return (
    <div className="text-center">
      <h2 style={{color: '#e5bbb1', fontWeight: 'bold'}}>{message}</h2>
      <ProgressBar 
        animated 
        now={progress} 
        className="mb-3" 
        style={{backgroundColor: '#d98c99'}}
      />
      <p style={{color: '#664960', fontSize: '1.1rem'}}>
        Lütfen bekleyin...
      </p>
    </div>
  );
};

export default LoadingSpinner; 