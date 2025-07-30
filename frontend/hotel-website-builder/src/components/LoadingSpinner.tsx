import React from 'react';
import { ProgressBar } from 'react-bootstrap';

interface LoadingSpinnerProps {
  message?: string;
  progress?: number;
  size?: 'sm' | 'md' | 'lg';
  type?: 'default' | 'clone' | 'generate';
  step?: number;
  totalSteps?: number;
}

const LoadingSpinner: React.FC<LoadingSpinnerProps> = ({ 
  message = "Yükleniyor...", 
  progress = 100,
  size = 'md',
  type = 'default',
  step = 1,
  totalSteps = 3
}) => {
  const sizeStyles = {
    sm: { fontSize: '0.875rem', padding: '0.5rem' },
    md: { fontSize: '1rem', padding: '1rem' },
    lg: { fontSize: '1.25rem', padding: '1.5rem' }
  };

  const currentStyle = sizeStyles[size];

  const getStepMessage = () => {
    if (type === 'clone') {
      const steps = [
        'Web sitesi analiz ediliyor...',
        'Otel bilgileri güncelleniyor...',
        'Asset\'ler indiriliyor...',
        'Site oluşturuluyor...'
      ];
      return steps[step - 1] || steps[steps.length - 1];
    }
    return message;
  };

  const getProgressPercentage = () => {
    if (type === 'clone') {
      return (step / totalSteps) * 100;
    }
    return progress;
  };

  return (
    <div className="text-center" style={currentStyle}>
      <h2 style={{color: '#e5bbb1', fontWeight: 'bold'}}>
        {type === 'clone' ? 'Site Klonlanıyor' : message}
      </h2>
      
      {type === 'clone' && (
        <div className="mb-3">
          <p style={{color: '#664960', fontSize: '1rem'}}>
            Adım {step} / {totalSteps}
          </p>
          <p style={{color: '#986277', fontSize: '0.9rem'}}>
            {getStepMessage()}
          </p>
        </div>
      )}
      
      <ProgressBar 
        animated 
        now={getProgressPercentage()} 
        className="mb-3" 
        style={{backgroundColor: '#d98c99'}}
      />
      
      <p style={{color: '#664960', fontSize: '1.1rem'}}>
        {type === 'clone' ? 'Bu işlem birkaç dakika sürebilir...' : 'Lütfen bekleyin...'}
      </p>
    </div>
  );
};

export default LoadingSpinner; 