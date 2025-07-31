import axios from 'axios';
import { 
  WebsiteBuilderRequest, 
  WebsiteBuilderResponse, 
  Hotel, 
  TemplateGenerationRequest,
  UrlGenerationRequest
} from '../types';

const API_BASE_URL = 'http://localhost:5001/api';
const BACKEND_BASE_URL = 'http://localhost:5001';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Hotels API
export const hotelsApi = {
  // Tüm otelleri getir
  getHotels: async (): Promise<Hotel[]> => {
    const response = await api.get<Hotel[]>('/hotels');
    return response.data;
  },

  // ID ile otel getir
  getHotelById: async (id: number): Promise<Hotel> => {
    const response = await api.get<Hotel>(`/hotels/${id}`);
    return response.data;
  },

  // Yeni otel oluştur
  createHotel: async (hotel: Omit<Hotel, 'id' | 'createdAt' | 'updatedAt'>): Promise<Hotel> => {
    const response = await api.post<Hotel>('/hotels', hotel);
    return response.data;
  },

  // Otel güncelle
  updateHotel: async (id: number, hotel: Hotel): Promise<Hotel> => {
    const response = await api.put<Hotel>(`/hotels/${id}`, hotel);
    return response.data;
  },

  // Otel sil
  deleteHotel: async (id: number): Promise<void> => {
    await api.delete(`/hotels/${id}`);
  },
};

// Website Builder API
export const websiteBuilderApi = {
  // Şablondan site üret
  generateFromTemplate: async (request: TemplateGenerationRequest): Promise<WebsiteBuilderResponse> => {
    const response = await api.post<WebsiteBuilderResponse>('/websitebuilder/generate/template', request);
    return response.data;
  },

  // URL'den site üret
  generateFromUrl: async (request: UrlGenerationRequest): Promise<WebsiteBuilderResponse> => {
    const response = await api.post<WebsiteBuilderResponse>('/websitebuilder/generate/from-url', request);
    return response.data;
  },

  // URL'den site klonla (yeni endpoint)
  generateFromUrlClone: async (request: UrlGenerationRequest): Promise<WebsiteBuilderResponse> => {
    const response = await api.post<WebsiteBuilderResponse>('/websitebuilder/generate/from-url-clone', request);
    return response.data;
  },

  // URL'den klonlama
  cloneFromUrl: async (url: string, hotelData: any): Promise<any> => {
    const response = await api.post('/websitebuilder/clone-from-url', {
      url: url,
      hotelData: hotelData
    });
    return response.data;
  },

  // Mevcut şablonları getir
  getTemplates: async (): Promise<string[]> => {
    const response = await api.get<string[]>('/websitebuilder/templates');
    return response.data;
  },

  // Eski build endpoint (geriye uyumluluk için)
  buildWebsite: async (request: WebsiteBuilderRequest): Promise<WebsiteBuilderResponse> => {
    const response = await api.post<WebsiteBuilderResponse>('/websitebuilder/build', request);
    return response.data;
  },

  // Otelleri getir
  getHotels: async (): Promise<Hotel[]> => {
    const response = await api.get<Hotel[]>('/hotels');
    return response.data;
  },

  // Tam site çıkarma (HTML, CSS, JS dahil)
  extractFullSite: async (url: string, hotelData: any): Promise<any> => {
    const response = await api.post('/websitebuilder/extract-full-site', {
      url: url,
      hotelData: hotelData
    });
    return response.data;
  },
};

export { BACKEND_BASE_URL };
export default api; 