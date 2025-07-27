import axios from 'axios';
import { 
  WebsiteBuilderRequest, 
  WebsiteBuilderResponse, 
  Hotel, 
  HotelSearchRequest,
  TemplateGenerationRequest,
  UrlGenerationRequest
} from '../types';

const API_BASE_URL = 'http://localhost:5001/api';

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
    const response = await api.post<WebsiteBuilderResponse>('/WebsiteBuilder/generate/template', request);
    return response.data;
  },

  // URL'den site üret
  generateFromUrl: async (request: UrlGenerationRequest): Promise<WebsiteBuilderResponse> => {
    const response = await api.post<WebsiteBuilderResponse>('/WebsiteBuilder/generate/from-url', request);
    return response.data;
  },

  // Mevcut şablonları getir
  getTemplates: async (): Promise<string[]> => {
    const response = await api.get<string[]>('/WebsiteBuilder/templates');
    return response.data;
  },

  // Eski build endpoint (geriye uyumluluk için)
  buildWebsite: async (request: WebsiteBuilderRequest): Promise<WebsiteBuilderResponse> => {
    const response = await api.post<WebsiteBuilderResponse>('/WebsiteBuilder/build', request);
    return response.data;
  },
};

export default api; 