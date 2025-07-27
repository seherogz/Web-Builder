import React, { useState, useEffect } from 'react';
import { 
  Row, 
  Col, 
  Card, 
  Button, 
  Form, 
  Alert, 
  Badge,
  ProgressBar 
} from 'react-bootstrap';
import { websiteBuilderApi } from '../services/api';

interface DesignSelectionProps {
  onNext: (template: string, url: string) => void;
}

const DesignSelection: React.FC<DesignSelectionProps> = ({ onNext }) => {
  const [designMethod, setDesignMethod] = useState<'template' | 'url'>('template');
  const [templates, setTemplates] = useState<string[]>([]);
  const [selectedTemplate, setSelectedTemplate] = useState<string>('');
  const [sourceUrl, setSourceUrl] = useState<string>('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    loadTemplates();
  }, []);

  const loadTemplates = async () => {
    try {
      setLoading(true);
      const availableTemplates = await websiteBuilderApi.getTemplates();
      setTemplates(availableTemplates);
      if (availableTemplates.length > 0) {
        setSelectedTemplate(availableTemplates[0]);
      }
    } catch (err) {
      setError('Şablonlar yüklenirken bir hata oluştu.');
      console.error('Template loading error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleNext = () => {
    if (designMethod === 'template' && !selectedTemplate) {
      setError('Lütfen bir şablon seçin.');
      return;
    }
    
    if (designMethod === 'url' && !sourceUrl) {
      setError('Lütfen bir URL girin.');
      return;
    }

    onNext(selectedTemplate, sourceUrl);
  };

  const templateDescriptions: { [key: string]: string } = {
    'modern': 'Modern ve şık tasarım, Bootstrap 5 ile oluşturulmuş',
    'classic': 'Klasik ve zarif tasarım, geleneksel otel siteleri için',
    'luxury': 'Lüks ve premium görünüm, yüksek kaliteli oteller için'
  };

  return (
    <div>
      <div className="text-center mb-4">
        <h2 style={{color: '#4c3949', fontWeight: 'bold'}}>Adım 1: Tasarım Seçenekleri</h2>
        <p style={{color: '#664960', fontSize: '1.1rem'}}>Web siteniz için tasarım yöntemini seçin</p>
      </div>

      {error && (
        <Alert variant="danger" dismissible onClose={() => setError('')}>
          {error}
        </Alert>
      )}

      <Row className="mb-4">
        <Col md={6}>
          <Card 
            className={`h-100 cursor-pointer ${designMethod === 'template' ? 'border-danger' : ''}`}
            style={{borderColor: designMethod === 'template' ? '#986277' : undefined}}
            onClick={() => setDesignMethod('template')}
          >
            <Card.Body className="text-center">
              <i className="fas fa-palette fa-3x mb-3" style={{color: '#986277'}}></i>
              <Card.Title>Hazır Şablon Seçimi</Card.Title>
              <Card.Text>
                Mevcut şablonlardan birini seçerek hızlıca başlayın.
              </Card.Text>
              <Badge bg={designMethod === 'template' ? 'danger' : 'secondary'}>
                {designMethod === 'template' ? 'Seçildi' : 'Seç'}
              </Badge>
            </Card.Body>
          </Card>
        </Col>
        <Col md={6}>
          <Card 
            className={`h-100 cursor-pointer ${designMethod === 'url' ? 'border-danger' : ''}`}
            style={{borderColor: designMethod === 'url' ? '#986277' : undefined}}
            onClick={() => setDesignMethod('url')}
          >
            <Card.Body className="text-center">
              <i className="fas fa-link fa-3x mb-3" style={{color: '#986277'}}></i>
              <Card.Title>URL ile Site Yapısı</Card.Title>
              <Card.Text>
                Beğendiğiniz bir otel sitesinin URL'sini girin.
              </Card.Text>
              <Badge bg={designMethod === 'url' ? 'danger' : 'secondary'}>
                {designMethod === 'url' ? 'Seçildi' : 'Seç'}
              </Badge>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {designMethod === 'template' && (
        <Card className="mb-4">
          <Card.Header>
            <h5 className="mb-0">Mevcut Şablonlar</h5>
          </Card.Header>
          <Card.Body>
            {loading ? (
              <div className="text-center">
                <ProgressBar animated now={100} />
                <p className="mt-2">Şablonlar yükleniyor...</p>
              </div>
            ) : (
              <Row>
                {templates.map((template) => (
                  <Col md={4} key={template} className="mb-3">
                    <Card 
                      className={`h-100 cursor-pointer ${selectedTemplate === template ? 'border-danger' : ''}`}
                      style={{borderColor: selectedTemplate === template ? '#986277' : undefined}}
                      onClick={() => setSelectedTemplate(template)}
                    >
                      <Card.Body className="text-center">
                        <i className="fas fa-image fa-2x text-muted mb-2"></i>
                        <Card.Title className="text-capitalize">{template}</Card.Title>
                        <Card.Text className="small">
                          {templateDescriptions[template] || 'Profesyonel otel web sitesi şablonu'}
                        </Card.Text>
                        {selectedTemplate === template && (
                          <Badge bg="danger">Seçildi</Badge>
                        )}
                      </Card.Body>
                    </Card>
                  </Col>
                ))}
              </Row>
            )}
          </Card.Body>
        </Card>
      )}

      {designMethod === 'url' && (
        <Card className="mb-4">
          <Card.Header>
            <h5 className="mb-0">URL Girişi</h5>
          </Card.Header>
          <Card.Body>
            <Form.Group>
              <Form.Label>Otel Web Sitesi URL'si</Form.Label>
              <Form.Control
                type="url"
                placeholder="https://example.com"
                value={sourceUrl}
                onChange={(e) => setSourceUrl(e.target.value)}
              />
              <Form.Text className="text-muted">
                Sistem bu URL'yi analiz ederek yapısını çıkaracak ve yeni sayfaya uyarlayacaktır.
              </Form.Text>
            </Form.Group>
          </Card.Body>
        </Card>
      )}

      <div className="text-center">
        <Button 
          variant="danger" 
          size="lg" 
          onClick={handleNext}
          disabled={loading}
          style={{backgroundColor: '#986277', borderColor: '#986277'}}
        >
          <i className="fas fa-arrow-right me-2"></i>
          Devam Et
        </Button>
      </div>
    </div>
  );
};

export default DesignSelection; 