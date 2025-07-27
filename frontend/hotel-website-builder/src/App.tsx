import React, { useState, useEffect } from 'react';
import { Container, Row, Col, Alert } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { 
  Header, 
  CommonCard, 
  CommonButton, 
  LoadingSpinner, 
  ErrorMessage,
  DesignSelection,
  ContentSource,
  WebsitePreview
} from './components';
import { WebsiteBuilderResponse, Hotel, WebsiteKeys } from './types';

function App() {
  const [currentStep, setCurrentStep] = useState(1); // 1 = Design Selection, 2 = Content Source, 3 = Website Preview
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
      
      let response: WebsiteBuilderResponse;

      if (selectedHotel?.id) {
        if (sourceUrl) {
          // URL'den üret
          response = await websiteBuilderApi.generateFromUrl({
            hotelId: selectedHotel.id,
            sourceUrl: sourceUrl
          });
        } else {
          // Şablondan üret
          response = await websiteBuilderApi.generateFromTemplate({
            hotelId: selectedHotel.id,
            templateName: selectedTemplate || 'modern'
          });
        }
      } else {
        // Eski yöntem (geriye uyumluluk)
        const request = {
          templateName: selectedTemplate || undefined,
          sourceUrl: sourceUrl || undefined,
          hotelId: selectedHotel?.id,
          hotelData: Object.keys(hotelData).length > 0 ? hotelData : undefined,
        };
        response = await websiteBuilderApi.buildWebsite(request);
      }

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
  }, [currentStep, selectedTemplate, sourceUrl, selectedHotel, hotelData]);

  return (
    <div className="App">
      <Container fluid className="py-4">
                <Row className="justify-content-center">
          <Col lg={10}>
            <CommonCard>
              <Header title="Otel Web Sitesi Oluşturucu" />
              <div className="p-4" style={{color: '#4c3949'}}>
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
              </div>
            </CommonCard>
          </Col>
        </Row>
      </Container>
    </div>
  );
}

export default App;
