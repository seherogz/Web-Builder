import React, { useState, useEffect } from 'react';
import { Container, Row, Col, Card, Alert } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import DesignSelection from './components/DesignSelection';
import ContentSource from './components/ContentSource';
import WebsitePreview from './components/WebsitePreview';
import { WebsiteBuilderResponse, Hotel, WebsiteKeys } from './types';

function App() {
  const [currentStep, setCurrentStep] = useState(1);
  const [selectedTemplate, setSelectedTemplate] = useState<string>('');
  const [sourceUrl, setSourceUrl] = useState<string>('');
  const [selectedHotel, setSelectedHotel] = useState<Hotel | null>(null);
  const [hotelData, setHotelData] = useState<WebsiteKeys>({});
  const [websiteResponse, setWebsiteResponse] = useState<WebsiteBuilderResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');

  const handleDesignSelection = (template: string, url: string) => {
    setSelectedTemplate(template);
    setSourceUrl(url);
    setCurrentStep(2);
  };

  const handleContentSource = (hotel: Hotel | null, data: WebsiteKeys) => {
    setSelectedHotel(hotel);
    setHotelData(data);
    setCurrentStep(3);
  };

  const generateWebsite = async () => {
    setLoading(true);
    setError('');

    try {
      const { websiteBuilderApi } = await import('./services/api');
      
      const request = {
        templateName: selectedTemplate || undefined,
        sourceUrl: sourceUrl || undefined,
        hotelId: selectedHotel?.id,
        hotelData: Object.keys(hotelData).length > 0 ? hotelData : undefined,
      };

      const response = await websiteBuilderApi.buildWebsite(request);
      setWebsiteResponse(response);
    } catch (err) {
      setError('Website oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.');
      console.error('Website generation error:', err);
    } finally {
      setLoading(false);
    }
  };

  const resetApp = () => {
    setCurrentStep(1);
    setSelectedTemplate('');
    setSourceUrl('');
    setSelectedHotel(null);
    setHotelData({});
    setWebsiteResponse(null);
    setError('');
  };

  useEffect(() => {
    if (currentStep === 3) {
      generateWebsite();
    }
  }, [currentStep]);

  return (
    <div className="App">
      <Container fluid className="py-4">
        <Row className="justify-content-center">
          <Col lg={10}>
            <Card className="shadow">
              <Card.Header className="bg-primary text-white">
                <h1 className="text-center mb-0">
                  <i className="fas fa-hotel me-2"></i>
                  Otel Web Sitesi Oluşturucu
                </h1>
              </Card.Header>
              <Card.Body className="p-4">
                {error && (
                  <Alert variant="danger" dismissible onClose={() => setError('')}>
                    {error}
                  </Alert>
                )}

                {currentStep === 1 && (
                  <DesignSelection onNext={handleDesignSelection} />
                )}

                {currentStep === 2 && (
                  <ContentSource 
                    onNext={handleContentSource}
                    onBack={() => setCurrentStep(1)}
                  />
                )}

                {currentStep === 3 && (
                  <WebsitePreview 
                    response={websiteResponse}
                    loading={loading}
                    onReset={resetApp}
                  />
                )}
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </Container>
    </div>
  );
}

export default App;
