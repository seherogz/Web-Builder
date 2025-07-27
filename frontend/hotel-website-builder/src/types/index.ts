export interface WebsiteKeys {
  hotelname?: string;
  logourl?: string;
  phone?: string;
  email?: string;
  address?: string;
  galleryimage1?: string;
  galleryimage2?: string;
  galleryimage3?: string;
  galleryimage4?: string;
  galleryimage5?: string;
  facebook?: string;
  instagram?: string;
  twitter?: string;
  website?: string;
  description?: string;
  amenities?: string;
  roomtypes?: string;
  pricing?: string;
}

export interface Hotel {
  id: number;
  hotelName: string;
  logoUrl?: string;
  phone: string;
  email: string;
  address: string;
  galleryImage1?: string;
  galleryImage2?: string;
  galleryImage3?: string;
  galleryImage4?: string;
  galleryImage5?: string;
  facebook?: string;
  instagram?: string;
  twitter?: string;
  website?: string;
  description?: string;
  amenities?: string;
  roomTypes?: string;
  pricing?: string;
  createdAt: string;
  updatedAt: string;
}

export interface WebsiteBuilderRequest {
  templateName?: string;
  sourceUrl?: string;
  hotelId?: number;
  hotelData?: WebsiteKeys;
}

export interface WebsiteBuilderResponse {
  htmlContent: string;
  websiteKeys: WebsiteKeys;
  templateName: string;
}

export interface HotelSearchRequest {
  hotelId?: number;
  hotelName?: string;
} 