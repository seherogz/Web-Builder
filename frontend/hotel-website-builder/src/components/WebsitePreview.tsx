import React, { useState, useRef, useEffect } from 'react';
import { 
  Row, 
  Col, 
  Card, 
  Button, 
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

  const openClonedSite = () => {
    if (response && response.outputPath) {
      const fullUrl = `${BACKEND_BASE_URL}${response.outputPath}`;
      window.open(fullUrl, '_blank');
    }
  };

  if (loading) {
    return (
      <div className="text-center" style={{ padding: '40px 20px' }}>
        <div style={{
          background: 'linear-gradient(135deg, #ffc2a4 0%, #ff8386 100%)',
          borderRadius: '20px',
          padding: '40px',
          boxShadow: '0 10px 30px rgba(76, 57, 73, 0.2)',
          margin: '20px 0'
        }}>
          <div style={{
            width: '80px',
            height: '80px',
            border: '4px solid #913856',
            borderTop: '4px solid transparent',
            borderRadius: '50%',
            animation: 'spin 1s linear infinite',
            margin: '0 auto 20px'
          }}></div>
          <h2 style={{color: '#4c3949', fontWeight: 'bold', marginBottom: '20px'}}>
            Website Oluşturuluyor...
          </h2>
          <ProgressBar 
            animated 
            now={100} 
            className="mb-3" 
            style={{
              backgroundColor: '#ff8386',
              height: '10px',
              borderRadius: '10px'
            }}
          />
          <p style={{color: '#913856', fontSize: '1.2rem', marginBottom: '10px'}}>
            Lütfen bekleyin, website hazırlanıyor.
          </p>
          <p style={{color: '#986277', fontSize: '1rem'}}>
            {response?.templateName === 'cloned' 
              ? 'Site klonlanıyor ve otel bilgileri güncelleniyor...' 
              : 'Website oluşturuluyor...'
            }
          </p>
        </div>
      </div>
    );
  }

  if (!response) {
    return (
      <div className="text-center" style={{ padding: '40px 20px' }}>
        <div style={{
          background: 'linear-gradient(135deg, #e5bbb1 0%, #d98c99 100%)',
          borderRadius: '20px',
          padding: '40px',
          boxShadow: '0 10px 30px rgba(76, 57, 73, 0.2)',
          margin: '20px 0'
        }}>
          <div style={{
            fontSize: '60px',
            marginBottom: '20px'
          }}></div>
          <h3 style={{color: '#4c3949', fontWeight: 'bold', marginBottom: '20px'}}>
            Hata Oluştu
          </h3>
          <p style={{color: '#664960', fontSize: '1.1rem', marginBottom: '30px'}}>
            Website oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.
          </p>
                      <Button 
              style={{
                backgroundColor: '#986277', 
                borderColor: '#986277', 
                color: '#e5bbb1',
                fontWeight: 'bold',
                padding: '12px 30px',
                borderRadius: '25px',
                fontSize: '1.1rem'
              }}
              variant="danger" 
              onClick={onReset}
            >
              Baştan Başla
            </Button>
        </div>
      </div>
    );
  }

  return (
    <div style={{ padding: '20px' }}>
      <div className="text-center mb-5">
        <h2 style={{
          color: '#4c3949', 
          fontWeight: 'bold',
          fontSize: '2.5rem',
          marginBottom: '15px',
          textShadow: '2px 2px 4px rgba(76, 57, 73, 0.1)'
        }}>
          {response.templateName === 'cloned' ? 'Site Klonlama Başarılı!' : 'Website Önizleme'}
        </h2>
        <p style={{
          color: '#664960', 
          fontSize: '1.3rem',
          marginBottom: '30px'
        }}>
          {response.templateName === 'cloned' 
            ? 'Klonlanan site başarıyla oluşturuldu ve hazır!' 
            : 'Oluşturulan website\'ı inceleyin ve indirin'
          }
        </p>
      </div>

      {response.templateName === 'cloned' && (
        <div style={{
          background: 'rgba(229, 187, 177, 0.95)',
          borderRadius: '20px',
          padding: '30px',
          marginBottom: '30px',
          border: '3px solid #986277',
          boxShadow: '0 8px 25px rgba(76, 57, 73, 0.2)'
        }}>
          <div style={{ textAlign: 'center', marginBottom: '25px' }}>
            <div style={{
              fontSize: '50px',
              marginBottom: '15px'
            }}></div>
            <h3 style={{
              color: '#4c3949',
              fontWeight: 'bold',
              marginBottom: '20px',
              fontSize: '2.5rem',
              textShadow: 'rgba(76, 57, 73, 0.1) 2px 2px 4px'
            }}>Site Başarıyla Klonlandı!</h3>
          </div>
          
          <Row>
            <Col md={6}>
              <div style={{
                background: 'rgba(229, 187, 177, 0.7)',
                borderRadius: '15px',
                padding: '20px',
                marginBottom: '15px'
              }}>
                <h5 style={{ color: '#4c3949', fontWeight: 'bold', marginBottom: '10px' }}>
                  Otel Bilgileri
                </h5>
                <p style={{ marginBottom: '8px' }}>
                  <strong>Otel:</strong> {response.websiteKeys.hotelname}
                </p>
                <p style={{ marginBottom: '8px' }}>
                  <strong>Telefon:</strong> {response.websiteKeys.phone}
                </p>
                <p style={{ marginBottom: '8px' }}>
                  <strong>E-posta:</strong> {response.websiteKeys.email}
                </p>
                <p style={{ marginBottom: '0' }}>
                  <strong>Adres:</strong> {response.websiteKeys.address}
                </p>
              </div>
            </Col>
            <Col md={6}>
              <div style={{
                background: 'rgba(229, 187, 177, 0.7)',
                borderRadius: '15px',
                padding: '20px',
                marginBottom: '15px'
              }}>
                <h5 style={{ color: '#4c3949', fontWeight: 'bold', marginBottom: '15px' }}>
                   Site Erişimi
                </h5>
                <p style={{ marginBottom: '15px' }}>
                  <strong>Site URL:</strong>
                </p>
                <Button
                  style={{
                    backgroundColor: '#986277',
                    borderColor: '#986277',
                    color: 'white',
                    fontWeight: 'bold',
                    padding: '12px 20px',
                    borderRadius: '25px',
                    fontSize: '1rem',
                    width: '100%',
                    transition: 'all 0.3s ease'
                  }}
                  variant="primary"
                  onClick={openClonedSite}
                  onMouseOver={(e) => {
                    (e.target as HTMLElement).style.backgroundColor = '#4c3949';
                    (e.target as HTMLElement).style.transform = 'translateY(-2px)';
                  }}
                  onMouseOut={(e) => {
                    (e.target as HTMLElement).style.backgroundColor = '#986277';
                    (e.target as HTMLElement).style.transform = 'translateY(0)';
                  }}
                                  >
                    Klonlanan Siteyi Aç
                  </Button>
                <p style={{
                  fontSize: '0.9rem',
                  color: '#4c3949',
                  marginTop: '10px',
                  marginBottom: '0'
                }}>
                  {`${BACKEND_BASE_URL}${response.outputPath}`}
                </p>
              </div>
            </Col>
          </Row>
          
          <div style={{
            background: 'rgba(229, 187, 177, 0.8)',
            borderRadius: '15px',
            padding: '20px',
            textAlign: 'center'
          }}>
            <p style={{
              color: '#4c3949',
              fontSize: '1.1rem',
              marginBottom: '0',
              fontWeight: 'bold'
            }}>
              Mesaj: Site başarıyla klonlandı ve otel bilgileri güncellendi.
            </p>
          </div>
        </div>
      )}

      <Row className="mb-4">
        <Col md={6}>
          <Card style={{
            backgroundColor: 'rgba(229, 187, 177, 0.9)', 
            borderColor: '#986277', 
            boxShadow: '0 8px 25px rgba(76, 57, 73, 0.2)',
            borderRadius: '20px',
            overflow: 'hidden'
          }}>
            <Card.Header style={{
              backgroundColor: '#4c3949', 
              color: '#e5bbb1', 
              borderBottom: '3px solid #986277',
              padding: '20px'
            }}>
              <h5 className="mb-0" style={{fontWeight: 'bold', fontSize: '1.3rem'}}>
                {response.templateName === 'cloned' ? 'Klonlanan Site Bilgileri' : '📋 Website Bilgileri'}
              </h5>
            </Card.Header>
            <Card.Body style={{
              color: '#4c3949', 
              backgroundColor: 'rgba(229, 187, 177, 0.7)',
              padding: '25px'
            }}>
              <div style={{ marginBottom: '15px' }}>
                <strong>Otel Adı:</strong> 
                <span style={{ marginLeft: '10px', fontWeight: 'normal' }}>
                  {response.websiteKeys.hotelname || 'Belirtilmemiş'}
                </span>
              </div>
              <div style={{ marginBottom: '15px' }}>
                <strong>Telefon:</strong> 
                <span style={{ marginLeft: '10px', fontWeight: 'normal' }}>
                  {response.websiteKeys.phone || 'Belirtilmemiş'}
                </span>
              </div>
              <div style={{ marginBottom: '15px' }}>
                <strong>E-posta:</strong> 
                <span style={{ marginLeft: '10px', fontWeight: 'normal' }}>
                  {response.websiteKeys.email || 'Belirtilmemiş'}
                </span>
              </div>
              <div style={{ marginBottom: '15px' }}>
                <strong>Adres:</strong> 
                <span style={{ marginLeft: '10px', fontWeight: 'normal' }}>
                  {response.websiteKeys.address || 'Belirtilmemiş'}
                </span>
              </div>
              <div style={{ marginBottom: '0' }}>
                <strong>🎨 Kullanılan Şablon:</strong> 
                <Badge style={{
                  backgroundColor: '#4c3949', 
                  color: '#e5bbb1',
                  fontWeight: 'bold',
                  padding: '8px 15px',
                  borderRadius: '20px',
                  fontSize: '0.9rem',
                  marginLeft: '10px'
                }} className="text-capitalize">
                  {response.templateName === 'cloned' ? 'Klonlanan Site' : response.templateName}
                </Badge>
              </div>
            </Card.Body>
          </Card>
        </Col>
        <Col md={6}>
          <Card style={{
            backgroundColor: 'rgba(229, 187, 177, 0.9)', 
            borderColor: '#986277', 
            boxShadow: '0 8px 25px rgba(76, 57, 73, 0.2)',
            borderRadius: '20px',
            overflow: 'hidden'
          }}>
            <Card.Header style={{
              backgroundColor: '#4c3949', 
              color: '#e5bbb1', 
              borderBottom: '3px solid #986277',
              padding: '20px'
            }}>
              <h5 className="mb-0" style={{fontWeight: 'bold', fontSize: '1.3rem'}}>⚙️ İşlemler</h5>
            </Card.Header>
            <Card.Body style={{
              color: '#4c3949', 
              backgroundColor: 'rgba(229, 187, 177, 0.7)',
              padding: '25px'
            }}>
              <div className="d-grid gap-3">
                <Button 
                  style={{
                    backgroundColor: '#986277', 
                    borderColor: '#986277', 
                    color: '#e5bbb1',
                    fontWeight: 'bold',
                    transition: 'all 0.3s ease',
                    borderRadius: '15px',
                    padding: '12px',
                    fontSize: '1rem'
                  }}
                  variant="danger" 
                  onClick={() => setShowPreview(true)}
                >
                  Önizleme
                </Button>
                <Button 
                  style={{
                    backgroundColor: '#664960', 
                    borderColor: '#664960', 
                    color: '#e5bbb1',
                    fontWeight: 'bold',
                    transition: 'all 0.3s ease',
                    borderRadius: '15px',
                    padding: '12px',
                    fontSize: '1rem'
                  }}
                  variant="secondary" 
                  onClick={() => setShowCode(true)}
                >
                  HTML Kodu Görüntüle
                </Button>
                <Button 
                  style={{
                    backgroundColor: '#d98c99', 
                    borderColor: '#d98c99', 
                    color: '#4c3949',
                    fontWeight: 'bold',
                    transition: 'all 0.3s ease',
                    borderRadius: '15px',
                    padding: '12px',
                    fontSize: '1rem'
                  }}
                  variant="warning" 
                  onClick={downloadHtml}
                >
                  HTML İndir
                </Button>
                <Button 
                  style={{
                    backgroundColor: '#6a2b49', 
                    borderColor: '#6a2b49', 
                    color: '#ffc2a4',
                    fontWeight: 'bold',
                    transition: 'all 0.3s ease',
                    borderRadius: '15px',
                    padding: '12px',
                    fontSize: '1rem'
                  }}
                  variant="dark" 
                  onClick={copyToClipboard}
                >
                  Kodu Kopyala
                </Button>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      <div className="text-center" style={{ marginTop: '40px' }}>
        <Button 
          style={{
            backgroundColor: '#6a2b49', 
            borderColor: '#6a2b49', 
            color: '#ffc2a4',
            fontWeight: 'bold',
            fontSize: '1.2rem',
            padding: '15px 40px',
            transition: 'all 0.3s ease',
            boxShadow: '0 8px 25px rgba(76, 57, 73, 0.3)',
            borderRadius: '30px'
          }}
          variant="dark" 
          size="lg" 
          onClick={onReset}
        >
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
            padding: '20px', 
            borderRadius: '15px',
            maxHeight: '60vh',
            overflowY: 'auto',
            fontFamily: 'monospace',
            fontSize: '12px',
            border: '3px solid #986277',
            boxShadow: 'inset 0 0 10px rgba(76, 57, 73, 0.1)'
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

      <style>{`
        @keyframes spin {
          0% { transform: rotate(0deg); }
          100% { transform: rotate(360deg); }
        }
      `}</style>
    </div>
  );
};

export default WebsitePreview; 