import React, { useState, useEffect, useCallback } from 'react';
import { Container, Row, Col, Alert } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { 
  Header, 
  CommonCard, 
  DesignSelection,
  ContentSource,
  WebsitePreview
} from './components';
import { WebsiteBuilderResponse, Hotel, WebsiteKeys } from './types';
import { BACKEND_BASE_URL } from './services/api';

function App() {
  const [currentStep, setCurrentStep] = useState(1); // 1 = Design Selection, 2 = Content Source, 3 = Website Preview
  const [selectedTemplate, setSelectedTemplate] = useState<string>('');
  const [sourceUrl, setSourceUrl] = useState<string>('');
  const [selectedHotel, setSelectedHotel] = useState<Hotel | null>(null);
  const [hotelData, setHotelData] = useState<WebsiteKeys>({});
  const [websiteResponse, setWebsiteResponse] = useState<WebsiteBuilderResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');
  const [isCloneMode, setIsCloneMode] = useState(false);

  const handleDesignSelection = (template: string, url: string) => {
    setSelectedTemplate(template);
    setSourceUrl(url);
    
    // Eğer URL girilmişse klonlama moduna geç
    if (url && url.trim() !== '') {
      setIsCloneMode(true);
      setCurrentStep(2);
    } else {
      setIsCloneMode(false);
      setCurrentStep(2);
    }
  };

  const handleContentSource = (hotel: Hotel | null, data: WebsiteKeys, cloneResponse?: any) => {
    setSelectedHotel(hotel);
    setHotelData(data);
    
    if (cloneResponse && isCloneMode) {
      // Klonlama modunda özel response
              setWebsiteResponse({
          htmlContent: `<html><body style="font-family: Arial, sans-serif; background: linear-gradient(135deg, #d4edda 0%, #c3e6cb 100%); margin: 0; padding: 40px; min-height: 100vh;"><div style="max-width: 800px; margin: 0 auto; background: white; border-radius: 20px; padding: 40px; box-shadow: 0 10px 30px rgba(0,0,0,0.1); text-align: center;"><div style="font-size: 60px; margin-bottom: 20px;"></div><h1 style="color: #155724; font-size: 2.5rem; margin-bottom: 20px;">Site Başarıyla Klonlandı!</h1><div style="background: #f8f9fa; border-radius: 15px; padding: 20px; margin: 20px 0;"><h3 style="color: #155724; margin-bottom: 15px;">Otel Bilgileri</h3><p style="margin: 10px 0; font-size: 1.1rem;"><strong>Otel:</strong> ${cloneResponse.hotelName}</p></div><div style="background: #e3f2fd; border-radius: 15px; padding: 20px; margin: 20px 0;"><h3 style="color: #1976d2; margin-bottom: 15px;">Site Erişimi</h3><a href="${BACKEND_BASE_URL}${cloneResponse.siteUrl}" target="_blank" style="display: inline-block; background: #007bff; color: white; padding: 15px 30px; text-decoration: none; border-radius: 25px; font-weight: bold; font-size: 1.1rem; transition: all 0.3s ease;">Klonlanan Siteyi Aç</a><p style="margin-top: 15px; color: #6c757d; font-size: 0.9rem;">${BACKEND_BASE_URL}${cloneResponse.siteUrl}</p></div><div style="background: #fff3cd; border-radius: 15px; padding: 20px; margin-top: 20px;"><p style="color: #856404; font-size: 1.1rem; margin: 0; font-weight: bold;">Mesaj: ${cloneResponse.message}</p></div></div></body></html>`,
        websiteKeys: data,
        templateName: 'cloned',
        outputPath: cloneResponse.outputDirectory || ''
      });
    }
    
    setCurrentStep(3);
  };

  const generateWebsite = useCallback(async () => {
    setLoading(true);
    setError('');

    try {
      const { websiteBuilderApi } = await import('./services/api');
      
      let response: WebsiteBuilderResponse;

      if (selectedHotel?.id) {
        // Mevcut otel seçilmişse
        if (sourceUrl) {
          // URL'den mevcut oteli klonla (yeni endpoint)
          response = await websiteBuilderApi.generateFromUrlClone({
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
      } else if (Object.keys(hotelData).length > 0 && hotelData.hotelname) {
        // Yeni otel verisi girilmişse
        if (sourceUrl) {
          // URL'den klonlama - yeni otel oluştur
          const cloneResponse = await websiteBuilderApi.cloneFromUrl(sourceUrl, hotelData);
          response = {
            htmlContent: `<html><body><h1>Site klonlandı: ${cloneResponse.outputPath}</h1></body></html>`,
            websiteKeys: hotelData,
            templateName: 'cloned',
            outputPath: cloneResponse.outputPath
          };
        } else {
          // Şablondan yeni otel oluştur
          const request = {
            templateName: selectedTemplate || 'modern',
            hotelData: hotelData
          };
          response = await websiteBuilderApi.buildWebsite(request);
        }
      } else {
        throw new Error('Otel verisi bulunamadı');
      }

      setWebsiteResponse(response);
    } catch (err) {
      setError('Website oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.');
      console.error('Website generation error:', err);
    } finally {
      setLoading(false);
    }
  }, [selectedHotel, sourceUrl, selectedTemplate, hotelData]);

  const resetApp = () => {
    setCurrentStep(1);
    setSelectedTemplate('');
    setSourceUrl('');
    setSelectedHotel(null);
    setHotelData({});
    setWebsiteResponse(null);
    setLoading(false);
    setError('');
    setIsCloneMode(false);
  };



  useEffect(() => {
    if (currentStep === 3) {
      generateWebsite();
    }
  }, [currentStep, generateWebsite]);

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
                    isCloneMode={isCloneMode}
                    sourceUrl={sourceUrl}
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
