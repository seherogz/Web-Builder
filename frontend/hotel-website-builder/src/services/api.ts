import axios from 'axios';
import { 
  WebsiteBuilderRequest, 
  WebsiteBuilderResponse, 
  Hotel, 
  HotelSearchRequest,
  WebsiteKeys 
} from '../types';

const API_BASE_URL = process.env.REACT_APP_API_URL || '/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const websiteBuilderApi = {
  // Website oluştur
  buildWebsite: async (request: WebsiteBuilderRequest): Promise<WebsiteBuilderResponse> => {
    const response = await api.post<WebsiteBuilderResponse>('/websitebuilder/build', request);
    return response.data;
  },

  // Mevcut şablonları getir
  getTemplates: async (): Promise<string[]> => {
    const response = await api.get<string[]>('/websitebuilder/templates');
    return response.data;
  },

  // Tüm otelleri getir
  getHotels: async (): Promise<Hotel[]> => {
    const response = await api.get<Hotel[]>('/websitebuilder/hotels');
    return response.data;
  },

  // ID ile otel getir
  getHotelById: async (id: number): Promise<Hotel> => {
    const response = await api.get<Hotel>(`/websitebuilder/hotels/${id}`);
    return response.data;
  },

  // Yeni otel oluştur
  createHotel: async (hotel: Omit<Hotel, 'id' | 'createdAt' | 'updatedAt'>): Promise<Hotel> => {
    const response = await api.post<Hotel>('/websitebuilder/hotels', hotel);
    return response.data;
  },

  // Otel ara
  searchHotel: async (request: HotelSearchRequest): Promise<Hotel> => {
    const response = await api.post<Hotel>('/websitebuilder/hotels/search', request);
    return response.data;
  },
};

export default api; 