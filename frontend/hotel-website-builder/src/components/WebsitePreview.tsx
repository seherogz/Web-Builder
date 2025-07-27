import React, { useState } from 'react';
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

interface WebsitePreviewProps {
  response: WebsiteBuilderResponse | null;
  loading: boolean;
  onReset: () => void;
}

const WebsitePreview: React.FC<WebsitePreviewProps> = ({ response, loading, onReset }) => {
  const [showPreview, setShowPreview] = useState(false);
  const [showCode, setShowCode] = useState(false);

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
        <h2>Website Oluşturuluyor...</h2>
        <ProgressBar animated now={100} className="mb-3" />
        <p className="text-muted">Lütfen bekleyin, website hazırlanıyor.</p>
      </div>
    );
  }

  if (!response) {
    return (
      <div className="text-center">
        <Alert variant="danger">
          Website oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.
        </Alert>
        <Button variant="primary" onClick={onReset}>
          Baştan Başla
        </Button>
      </div>
    );
  }

  return (
    <div>
      <div className="text-center mb-4">
        <h2>Adım 3: Website Önizleme</h2>
        <p className="text-muted">Oluşturulan website'ı inceleyin ve indirin</p>
      </div>

      <Row className="mb-4">
        <Col md={6}>
          <Card>
            <Card.Header>
              <h5 className="mb-0">Website Bilgileri</h5>
            </Card.Header>
            <Card.Body>
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
                <Badge bg="info" className="ms-2 text-capitalize">
                  {response.templateName}
                </Badge>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col md={6}>
          <Card>
            <Card.Header>
              <h5 className="mb-0">İşlemler</h5>
            </Card.Header>
            <Card.Body>
              <div className="d-grid gap-2">
                <Button 
                  variant="outline-primary" 
                  onClick={() => setShowPreview(true)}
                >
                  <i className="fas fa-eye me-2"></i>
                  Önizleme
                </Button>
                <Button 
                  variant="outline-secondary" 
                  onClick={() => setShowCode(true)}
                >
                  <i className="fas fa-code me-2"></i>
                  HTML Kodu Görüntüle
                </Button>
                <Button 
                  variant="success" 
                  onClick={downloadHtml}
                >
                  <i className="fas fa-download me-2"></i>
                  HTML İndir
                </Button>
                <Button 
                  variant="info" 
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
        <Button variant="primary" size="lg" onClick={onReset}>
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
        <Modal.Header closeButton>
          <Modal.Title>Website Önizleme</Modal.Title>
        </Modal.Header>
        <Modal.Body className="p-0">
          <iframe
            srcDoc={response.htmlContent}
            style={{ width: '100%', height: '80vh', border: 'none' }}
            title="Website Preview"
          />
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowPreview(false)}>
            Kapat
          </Button>
          <Button variant="primary" onClick={downloadHtml}>
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
        <Modal.Header closeButton>
          <Modal.Title>HTML Kodu</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <div style={{ 
            backgroundColor: '#f8f9fa', 
            padding: '15px', 
            borderRadius: '5px',
            maxHeight: '60vh',
            overflowY: 'auto',
            fontFamily: 'monospace',
            fontSize: '12px'
          }}>
            <pre style={{ margin: 0, whiteSpace: 'pre-wrap' }}>
              {response.htmlContent}
            </pre>
          </div>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowCode(false)}>
            Kapat
          </Button>
          <Button variant="info" onClick={copyToClipboard}>
            Kopyala
          </Button>
          <Button variant="success" onClick={downloadHtml}>
            İndir
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};

export default WebsitePreview; 