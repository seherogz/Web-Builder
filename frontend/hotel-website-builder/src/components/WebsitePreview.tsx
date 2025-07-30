import React, { useState, useRef, useEffect } from 'react';
import { 
  Row, 
  Col, 
  Card, 
  Button, 
  Alert, 
  ProgressBar,
  Badge,
  Modal
} from 'react-bootstrap';
import { WebsiteBuilderResponse } from '../types';
import { BACKEND_BASE_URL } from '../services/api';

interface WebsitePreviewProps {
  response: WebsiteBuilderResponse | null;
  loading: boolean;
  onReset: () => void;
}

const WebsitePreview: React.FC<WebsitePreviewProps> = ({ response, loading, onReset }) => {
  const [showPreview, setShowPreview] = useState(false);
  const [showCode, setShowCode] = useState(false);
  const previewRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (showPreview && response && previewRef.current) {
      // HTML içeriğini doğrudan render et
      previewRef.current.innerHTML = response.htmlContent;
    }
  }, [showPreview, response]);

  const downloadHtml = () => {
    if (!response) return;
    
    const blob = new Blob([response.htmlContent], { type: 'text/html' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `${response.websiteKeys.hotelname || 'hotel'}-website.html`;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
  };

  const copyToClipboard = async () => {
    if (!response) return;
    
    try {
      await navigator.clipboard.writeText(response.htmlContent);
      alert('HTML kodu panoya kopyalandı!');
    } catch (err) {
      console.error('Copy failed:', err);
      alert('Kopyalama başarısız oldu.');
    }
  };

  if (loading) {
    return (
      <div className="text-center">
        <h2 style={{color: '#ffc2a4', fontWeight: 'bold'}}>Website Oluşturuluyor...</h2>
        <ProgressBar 
          animated 
          now={100} 
          className="mb-3" 
          style={{backgroundColor: '#ff8386'}}
        />
        <p style={{color: '#913856', fontSize: '1.1rem'}}>Lütfen bekleyin, website hazırlanıyor.</p>
        <p style={{color: '#986277', fontSize: '0.9rem'}}>
          {response?.templateName === 'url_cloned' 
            ? 'Site klonlanıyor ve otel bilgileri güncelleniyor...' 
            : 'Website oluşturuluyor...'
          }
        </p>
      </div>
    );
  }

  if (!response) {
    return (
      <div className="text-center">
        <Alert variant="danger" style={{backgroundColor: '#e5bbb1', borderColor: '#986277', color: '#4c3949'}}>
          Website oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.
        </Alert>
        <Button 
          style={{backgroundColor: '#986277', borderColor: '#986277', color: '#e5bbb1'}}
          variant="danger" 
          onClick={onReset}
        >
          Baştan Başla
        </Button>
      </div>
    );
  }

  return (
    <div>
      <div className="text-center mb-4">
        <h2 style={{color: '#4c3949', fontWeight: 'bold'}}>
          {response.templateName === 'cloned' ? 'Adım 3: Site Klonlama Sonucu' : 'Adım 3: Website Önizleme'}
        </h2>
        <p style={{color: '#664960', fontSize: '1.1rem'}}>
          {response.templateName === 'cloned' 
            ? 'Klonlanan site başarıyla oluşturuldu' 
            : 'Oluşturulan website\'ı inceleyin ve indirin'
          }
        </p>
      </div>

      {response.templateName === 'cloned' && (
        <Alert variant="success" className="mb-4">
          <h4>✅ Site Başarıyla Klonlandı!</h4>
          <p><strong>Otel:</strong> {response.websiteKeys.hotelname}</p>
          <p><strong>Site URL:</strong> <a 
            href={`${BACKEND_BASE_URL}${response.outputPath}`} 
            target="_blank" 
            rel="noopener noreferrer" 
            style={{
              color: '#0066cc', 
              textDecoration: 'underline',
              fontWeight: 'bold',
              cursor: 'pointer'
            }}
            onMouseOver={(e) => (e.target as HTMLElement).style.color = '#003366'}
            onMouseOut={(e) => (e.target as HTMLElement).style.color = '#0066cc'}
          >
            {response.outputPath}
          </a></p>
          <p><strong>Mesaj:</strong> Site başarıyla klonlandı ve otel bilgileri güncellendi.</p>
        </Alert>
      )}

      <Row className="mb-4">
        <Col md={6}>
          <Card style={{backgroundColor: 'rgba(229, 187, 177, 0.9)', borderColor: '#986277', boxShadow: '0 4px 15px rgba(76, 57, 73, 0.2)'}}>
            <Card.Header style={{backgroundColor: '#4c3949', color: '#e5bbb1', borderBottom: '2px solid #986277'}}>
              <h5 className="mb-0" style={{fontWeight: 'bold'}}>
                {response.templateName === 'cloned' ? 'Klonlanan Site Bilgileri' : 'Website Bilgileri'}
              </h5>
            </Card.Header>
            <Card.Body style={{color: '#4c3949', backgroundColor: 'rgba(229, 187, 177, 0.7)'}}>
              <div className="mb-3">
                <strong>Otel Adı:</strong> {response.websiteKeys.hotelname || 'Belirtilmemiş'}
              </div>
              <div className="mb-3">
                <strong>Telefon:</strong> {response.websiteKeys.phone || 'Belirtilmemiş'}
              </div>
              <div className="mb-3">
                <strong>E-posta:</strong> {response.websiteKeys.email || 'Belirtilmemiş'}
              </div>
              <div className="mb-3">
                <strong>Adres:</strong> {response.websiteKeys.address || 'Belirtilmemiş'}
              </div>
              <div className="mb-3">
                <strong>Kullanılan Şablon:</strong> 
                <Badge style={{
                  backgroundColor: '#4c3949', 
                  color: '#e5bbb1',
                  fontWeight: 'bold',
                  padding: '6px 12px',
                  borderRadius: '15px'
                }} className="ms-2 text-capitalize">
                  {response.templateName === 'cloned' ? 'Klonlanan Site' : response.templateName}
                </Badge>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col md={6}>
          <Card style={{backgroundColor: 'rgba(229, 187, 177, 0.9)', borderColor: '#986277', boxShadow: '0 4px 15px rgba(76, 57, 73, 0.2)'}}>
            <Card.Header style={{backgroundColor: '#4c3949', color: '#e5bbb1', borderBottom: '2px solid #986277'}}>
              <h5 className="mb-0" style={{fontWeight: 'bold'}}>İşlemler</h5>
            </Card.Header>
            <Card.Body style={{color: '#4c3949', backgroundColor: 'rgba(229, 187, 177, 0.7)'}}>
              <div className="d-grid gap-2">
                <Button 
                  style={{
                    backgroundColor: '#986277', 
                    borderColor: '#986277', 
                    color: '#e5bbb1',
                    fontWeight: 'bold',
                    transition: 'all 0.3s ease'
                  }}
                  variant="danger" 
                  onClick={() => setShowPreview(true)}
                >
                  <i className="fas fa-eye me-2"></i>
                  Önizleme
                </Button>
                <Button 
                  style={{
                    backgroundColor: '#664960', 
                    borderColor: '#664960', 
                    color: '#e5bbb1',
                    fontWeight: 'bold',
                    transition: 'all 0.3s ease'
                  }}
                  variant="secondary" 
                  onClick={() => setShowCode(true)}
                >
                  <i className="fas fa-code me-2"></i>
                  HTML Kodu Görüntüle
                </Button>
                <Button 
                  style={{
                    backgroundColor: '#d98c99', 
                    borderColor: '#d98c99', 
                    color: '#4c3949',
                    fontWeight: 'bold',
                    transition: 'all 0.3s ease'
                  }}
                  variant="warning" 
                  onClick={downloadHtml}
                >
                  <i className="fas fa-download me-2"></i>
                  HTML İndir
                </Button>
                <Button 
                  style={{
                    backgroundColor: '#6a2b49', 
                    borderColor: '#6a2b49', 
                    color: '#ffc2a4',
                    fontWeight: 'bold',
                    transition: 'all 0.3s ease'
                  }}
                  variant="dark" 
                  onClick={copyToClipboard}
                >
                  <i className="fas fa-copy me-2"></i>
                  Kodu Kopyala
                </Button>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      <div className="text-center">
        <Button 
          style={{
            backgroundColor: '#6a2b49', 
            borderColor: '#6a2b49', 
            color: '#ffc2a4',
            fontWeight: 'bold',
            fontSize: '1.1rem',
            padding: '12px 30px',
            transition: 'all 0.3s ease',
            boxShadow: '0 4px 15px rgba(76, 57, 73, 0.3)'
          }}
          variant="dark" 
          size="lg" 
          onClick={onReset}
        >
          <i className="fas fa-redo me-2"></i>
          Yeni Website Oluştur
        </Button>
      </div>

      {/* Website Önizleme Modal */}
      <Modal 
        show={showPreview} 
        onHide={() => setShowPreview(false)} 
        size="xl"
        fullscreen
      >
        <Modal.Header closeButton style={{backgroundColor: '#4c3949', color: '#e5bbb1'}}>
          <Modal.Title>Website Önizleme</Modal.Title>
        </Modal.Header>
        <Modal.Body className="p-0">
          <div 
            ref={previewRef}
            style={{ 
              width: '100%', 
              height: '80vh', 
              border: 'none',
              overflow: 'auto'
            }}
          />
        </Modal.Body>
        <Modal.Footer style={{backgroundColor: '#e5bbb1'}}>
          <Button 
            style={{borderColor: '#664960', color: '#664960'}}
            variant="outline-secondary" 
            onClick={() => setShowPreview(false)}
          >
            Kapat
          </Button>
          <Button 
            style={{backgroundColor: '#986277', borderColor: '#986277', color: '#e5bbb1'}}
            variant="danger" 
            onClick={downloadHtml}
          >
            İndir
          </Button>
        </Modal.Footer>
      </Modal>

      {/* HTML Kodu Modal */}
      <Modal 
        show={showCode} 
        onHide={() => setShowCode(false)} 
        size="xl"
      >
        <Modal.Header closeButton style={{backgroundColor: '#4c3949', color: '#e5bbb1'}}>
          <Modal.Title>HTML Kodu</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <div style={{ 
            backgroundColor: '#e5bbb1', 
            padding: '15px', 
            borderRadius: '5px',
            maxHeight: '60vh',
            overflowY: 'auto',
            fontFamily: 'monospace',
            fontSize: '12px',
            border: '2px solid #986277'
          }}>
            <pre style={{ margin: 0, whiteSpace: 'pre-wrap' }}>
              {response.htmlContent}
            </pre>
          </div>
        </Modal.Body>
        <Modal.Footer style={{backgroundColor: '#ffc2a4'}}>
          <Button 
            style={{borderColor: '#913856', color: '#913856'}}
            variant="outline-secondary" 
            onClick={() => setShowCode(false)}
          >
            Kapat
          </Button>
          <Button 
            style={{backgroundColor: '#6a2b49', borderColor: '#6a2b49', color: '#ffc2a4'}}
            variant="dark" 
            onClick={copyToClipboard}
          >
            Kopyala
          </Button>
          <Button 
            style={{backgroundColor: '#ff8386', borderColor: '#ff8386', color: '#6a2b49'}}
            variant="warning" 
            onClick={downloadHtml}
          >
            İndir
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};

export default WebsitePreview; 