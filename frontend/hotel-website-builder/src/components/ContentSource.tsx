import React, { useState, useEffect } from 'react';
import { 
  Row, 
  Col, 
  Card, 
  Button, 
  Form, 
  Alert, 
  Badge,
  ProgressBar,
  Table,
  Modal
} from 'react-bootstrap';
import { hotelsApi } from '../services/api';
import api from '../services/api';
import { Hotel, WebsiteKeys } from '../types';

interface ContentSourceProps {
  onNext: (hotel: Hotel | null, data: WebsiteKeys, cloneResponse?: any) => void;
  onBack: () => void;
  isCloneMode?: boolean;
  sourceUrl?: string;
}

const ContentSource: React.FC<ContentSourceProps> = ({ onNext, onBack, isCloneMode = false, sourceUrl = '' }) => {
  const [contentMethod, setContentMethod] = useState<'existing' | 'new'>('existing');
  const [hotels, setHotels] = useState<Hotel[]>([]);
  const [selectedHotel, setSelectedHotel] = useState<Hotel | null>(null);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [hotelData, setHotelData] = useState<WebsiteKeys>({});
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');
  const [showNewHotelModal, setShowNewHotelModal] = useState(false);

  useEffect(() => {
    loadHotels();
  }, []);

  const loadHotels = async () => {
    try {
      setLoading(true);
      const availableHotels = await hotelsApi.getHotels();
      setHotels(availableHotels);
    } catch (err) {
      setError('Oteller yüklenirken bir hata oluştu.');
      console.error('Hotel loading error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleHotelSelect = (hotel: Hotel) => {
    setSelectedHotel(hotel);
    // Hotel verilerini WebsiteKeys formatına çevir
    const websiteKeys: WebsiteKeys = {
      hotelname: hotel.hotelName,
      logourl: hotel.logoUrl,
      phone: hotel.phone,
      email: hotel.email,
      address: hotel.address,
      galleryimage1: hotel.galleryImage1,
      galleryimage2: hotel.galleryImage2,
      galleryimage3: hotel.galleryImage3,
      galleryimage4: hotel.galleryImage4,
      galleryimage5: hotel.galleryImage5,
      facebook: hotel.facebook,
      instagram: hotel.instagram,
      twitter: hotel.twitter,
      website: hotel.website,
      description: hotel.description,
      amenities: hotel.amenities,
      roomtypes: hotel.roomTypes,
      pricing: hotel.pricing
    };
    setHotelData(websiteKeys);
  };

  const handleNewHotelSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    
    try {
      setLoading(true);
      const newHotel = await hotelsApi.createHotel({
        hotelName: hotelData.hotelname || '',
        logoUrl: hotelData.logourl,
        phone: hotelData.phone || '',
        email: hotelData.email || '',
        address: hotelData.address || '',
        galleryImage1: hotelData.galleryimage1,
        galleryImage2: hotelData.galleryimage2,
        galleryImage3: hotelData.galleryimage3,
        galleryImage4: hotelData.galleryimage4,
        galleryImage5: hotelData.galleryimage5,
        facebook: hotelData.facebook,
        instagram: hotelData.instagram,
        twitter: hotelData.twitter,
        website: hotelData.website,
        description: hotelData.description,
        amenities: hotelData.amenities,
        roomTypes: hotelData.roomtypes,
        pricing: hotelData.pricing
      });
      
      setSelectedHotel(newHotel);
      setShowNewHotelModal(false);
      await loadHotels(); // Listeyi yenile
    } catch (err) {
      setError('Otel kaydedilirken bir hata oluştu.');
      console.error('Hotel creation error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleNext = () => {
    if (contentMethod === 'existing' && !selectedHotel) {
      setError('Lütfen bir otel seçin.');
      return;
    }
    
    if (contentMethod === 'new' && !hotelData.hotelname) {
      setError('Lütfen otel adını girin.');
      return;
    }

    // Eğer klonlama modundaysa ve URL varsa, doğrudan klonlama yap
    if (isCloneMode && sourceUrl) {
      handleClone();
    } else {
      onNext(selectedHotel, hotelData);
    }
  };

  const handleClone = async () => {
    if (!sourceUrl.trim()) {
      setError('Kaynak URL bulunamadı.');
      return;
    }

    if (contentMethod === 'existing' && !selectedHotel) {
      setError('Lütfen bir otel seçin.');
      return;
    }

    if (contentMethod === 'new' && !hotelData.hotelname) {
      setError('Lütfen otel adını girin.');
      return;
    }

    setLoading(true);
    setError('');

    try {
      const { websiteBuilderApi } = await import('../services/api');
      
      let response;
      
      if (contentMethod === 'existing' && selectedHotel) {
        // Mevcut otel ile klonlama (yeni endpoint)
        console.log('Klonlama başlatılıyor:', {
          hotelId: selectedHotel.id,
          sourceUrl: sourceUrl.trim(),
          hotelName: selectedHotel.hotelName
        });
        
        response = await websiteBuilderApi.generateFromUrlClone({
          hotelId: selectedHotel.id,
          sourceUrl: sourceUrl.trim()
        });
      } else {
        // Yeni otel ile klonlama
        response = await websiteBuilderApi.cloneFromUrl(sourceUrl.trim(), hotelData);
      }

      console.log('Klonlama sonucu:', response);

      if (response.outputPath) {
        // Başarılı klonlama sonrası preview'e git
        onNext(selectedHotel, hotelData, {
          success: true,
          siteUrl: response.outputPath,
          hotelName: selectedHotel?.hotelName || hotelData.hotelname,
          message: 'Site başarıyla klonlandı'
        });
      } else {
        setError('Site klonlanırken bir hata oluştu');
      }
    } catch (err: any) {
      console.error('Clone error:', err);
      const errorMessage = err.response?.data?.message || err.message || 'Site klonlanırken bir hata oluştu';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  const filteredHotels = hotels.filter(hotel =>
    hotel.hotelName.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div>
      <div className="text-center mb-4">
        <h2 style={{color: '#4c3949', fontWeight: 'bold'}}>
          {isCloneMode ? 'Adım 2: Otel Seçimi ve Klonlama' : 'Adım 2: İçerik Kaynağı'}
        </h2>
        <p style={{color: '#664960', fontSize: '1.1rem'}}>
          {isCloneMode 
            ? 'Klonlanacak oteli seçin veya yeni otel bilgilerini girin' 
            : 'Otel bilgilerini nasıl gireceğinizi seçin'
          }
        </p>
        {isCloneMode && sourceUrl && (
          <Alert variant="info" className="mt-3">
            <strong>Kaynak URL:</strong> {sourceUrl}
          </Alert>
        )}
      </div>

      {error && (
        <Alert variant="danger" dismissible onClose={() => setError('')}>
          {error}
        </Alert>
      )}

      <Row className="mb-4">
        <Col md={6}>
          <Card 
            className={`h-100 cursor-pointer ${contentMethod === 'existing' ? 'border-danger' : ''}`}
            style={{borderColor: contentMethod === 'existing' ? '#986277' : undefined}}
            onClick={() => setContentMethod('existing')}
          >
            <Card.Body className="text-center">
              <i className="fas fa-database fa-3x mb-3" style={{color: '#986277'}}></i>
              <Card.Title>Mevcut Otel</Card.Title>
              <Card.Text>
                Veritabanından kayıtlı bir otel seçin. URL girilirse otel bilgileri URL'den güncellenecek.
              </Card.Text>
              <Badge bg={contentMethod === 'existing' ? 'danger' : 'secondary'}>
                {contentMethod === 'existing' ? 'Seçildi' : 'Seç'}
              </Badge>
            </Card.Body>
          </Card>
        </Col>
        <Col md={6}>
          <Card 
            className={`h-100 cursor-pointer ${contentMethod === 'new' ? 'border-danger' : ''}`}
            style={{borderColor: contentMethod === 'new' ? '#986277' : undefined}}
            onClick={() => setContentMethod('new')}
          >
            <Card.Body className="text-center">
              <i className="fas fa-plus fa-3x mb-3" style={{color: '#986277'}}></i>
              <Card.Title>Yeni Otel</Card.Title>
              <Card.Text>
                Yeni otel bilgilerini girin. URL girilirse yeni otel URL'den klonlanacak.
              </Card.Text>
              <Badge bg={contentMethod === 'new' ? 'danger' : 'secondary'}>
                {contentMethod === 'new' ? 'Seçildi' : 'Seç'}
              </Badge>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      {contentMethod === 'existing' && (
        <Card className="mb-4">
          <Card.Header>
            <h5 className="mb-0">Mevcut Oteller</h5>
          </Card.Header>
          <Card.Body>
            <Form.Group className="mb-3">
              <Form.Control
                type="text"
                placeholder="Otel adı ile ara..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
            </Form.Group>
            
            {loading ? (
              <div className="text-center">
                <ProgressBar animated now={100} />
                <p className="mt-2">Oteller yükleniyor...</p>
              </div>
            ) : (
              <div style={{ maxHeight: '400px', overflowY: 'auto' }}>
                <Table striped hover>
                  <thead>
                    <tr>
                      <th>Otel Adı</th>
                      <th>Telefon</th>
                      <th>E-posta</th>
                      <th>İşlem</th>
                    </tr>
                  </thead>
                  <tbody>
                    {filteredHotels.map((hotel) => (
                      <tr 
                        key={hotel.id}
                        className={selectedHotel?.id === hotel.id ? 'table-primary' : ''}
                        onClick={() => handleHotelSelect(hotel)}
                        style={{ cursor: 'pointer' }}
                      >
                        <td>{hotel.hotelName}</td>
                        <td>{hotel.phone}</td>
                        <td>{hotel.email}</td>
                        <td>
                          {selectedHotel?.id === hotel.id && (
                            <Badge bg="success">Seçildi</Badge>
                          )}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </Table>
              </div>
            )}
          </Card.Body>
        </Card>
      )}

      {contentMethod === 'new' && (
        <Card className="mb-4">
          <Card.Header>
            <h5 className="mb-0">Yeni Otel Bilgileri</h5>
          </Card.Header>
          <Card.Body>
            <Button 
              variant="outline-danger" 
              onClick={() => setShowNewHotelModal(true)}
              className="w-100"
              style={{borderColor: '#cb5367', color: '#cb5367'}}
            >
              <i className="fas fa-plus me-2"></i>
              Yeni Otel Ekle
            </Button>
          </Card.Body>
        </Card>
      )}

      <div className="d-flex justify-content-between">
        <Button variant="outline-secondary" onClick={onBack}>
          <i className="fas fa-arrow-left me-2"></i>
          Geri
        </Button>
        <Button 
          variant="danger" 
          size="lg" 
          onClick={handleNext}
          disabled={loading}
          style={{backgroundColor: '#986277', borderColor: '#986277'}}
        >
          <i className="fas fa-arrow-right me-2"></i>
          {isCloneMode ? 'Site Klonla' : 'Devam Et'}
        </Button>
      </div>

      {/* Yeni Otel Modal */}
      <Modal show={showNewHotelModal} onHide={() => setShowNewHotelModal(false)} size="lg">
        <Modal.Header closeButton>
          <Modal.Title>Yeni Otel Ekle</Modal.Title>
        </Modal.Header>
        <Form onSubmit={handleNewHotelSubmit}>
          <Modal.Body>
            <Row>
              <Col md={6}>
                <Form.Group className="mb-3">
                  <Form.Label>Otel Adı *</Form.Label>
                  <Form.Control
                    type="text"
                    value={hotelData.hotelname || ''}
                    onChange={(e) => setHotelData({...hotelData, hotelname: e.target.value})}
                    required
                  />
                </Form.Group>
              </Col>
              <Col md={6}>
                <Form.Group className="mb-3">
                  <Form.Label>Logo URL</Form.Label>
                  <Form.Control
                    type="url"
                    value={hotelData.logourl || ''}
                    onChange={(e) => setHotelData({...hotelData, logourl: e.target.value})}
                  />
                </Form.Group>
              </Col>
            </Row>
            <Row>
              <Col md={6}>
                <Form.Group className="mb-3">
                  <Form.Label>Telefon *</Form.Label>
                  <Form.Control
                    type="tel"
                    value={hotelData.phone || ''}
                    onChange={(e) => setHotelData({...hotelData, phone: e.target.value})}
                    required
                  />
                </Form.Group>
              </Col>
              <Col md={6}>
                <Form.Group className="mb-3">
                  <Form.Label>E-posta *</Form.Label>
                  <Form.Control
                    type="email"
                    value={hotelData.email || ''}
                    onChange={(e) => setHotelData({...hotelData, email: e.target.value})}
                    required
                  />
                </Form.Group>
              </Col>
            </Row>
            <Form.Group className="mb-3">
              <Form.Label>Adres *</Form.Label>
              <Form.Control
                type="text"
                value={hotelData.address || ''}
                onChange={(e) => setHotelData({...hotelData, address: e.target.value})}
                required
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Açıklama</Form.Label>
              <Form.Control
                as="textarea"
                rows={3}
                value={hotelData.description || ''}
                onChange={(e) => setHotelData({...hotelData, description: e.target.value})}
              />
            </Form.Group>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={() => setShowNewHotelModal(false)}>
              İptal
            </Button>
            <Button variant="primary" type="submit" disabled={loading}>
              {loading ? 'Kaydediliyor...' : 'Kaydet'}
            </Button>
          </Modal.Footer>
        </Form>
      </Modal>
    </div>
  );
};

export default ContentSource; 