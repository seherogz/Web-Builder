import React from 'react';
import { Alert } from 'react-bootstrap';
import CommonButton from './CommonButton';

interface ErrorMessageProps {
  message: string;
  onRetry?: () => void;
  retryText?: string;
}

const ErrorMessage: React.FC<ErrorMessageProps> = ({ 
  message, 
  onRetry, 
  retryText = "Tekrar Dene" 
}) => {
  return (
    <div className="text-center">
      <Alert 
        variant="danger" 
        style={{
          backgroundColor: '#e5bbb1', 
          borderColor: '#986277', 
          color: '#4c3949'
        }}
      >
        {message}
      </Alert>
      {onRetry && (
        <CommonButton onClick={onRetry}>
          {retryText}
        </CommonButton>
      )}
    </div>
  );
};

export default ErrorMessage; 