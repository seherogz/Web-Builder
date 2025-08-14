using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace HotelWebsiteBuilder.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        
        [Required]
        public string HotelName { get; set; } = string.Empty;
        
        public string? LogoUrl { get; set; }
        
        [Required]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Address { get; set; } = string.Empty;
        
        public string? Website { get; set; }
        
        // Gallery images as JSON array
        public string? GalleryImagesJson { get; set; }
        
        // Slider images as JSON array
        public string? SliderImagesJson { get; set; }
        
        // Social media links
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? LinkedIn { get; set; }
        public string? YouTube { get; set; }
        
        // Basic info
        public string? Description { get; set; }
        public string? AmenitiesJson { get; set; }
        
        // Rooms as JSON array
        public string? RoomsJson { get; set; }
        
        // Facilities as JSON array
        public string? FacilitiesJson { get; set; }
        
        // Meta information
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        
        // Location
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        
        // Additional information
        public int StarRating { get; set; } = 4;
        public string Currency { get; set; } = "TRY";
        public string Language { get; set; } = "tr";
        
        // Legacy fields for backward compatibility
        public string? GalleryImage1 { get; set; }
        public string? GalleryImage2 { get; set; }
        public string? GalleryImage3 { get; set; }
        public string? GalleryImage4 { get; set; }
        public string? GalleryImage5 { get; set; }
        public string? RoomTypes { get; set; }
        public string? Pricing { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Helper methods to work with JSON data
        public List<string> GetGalleryImages()
        {
            if (string.IsNullOrEmpty(GalleryImagesJson))
                return new List<string>();
            
            try
            {
                return JsonSerializer.Deserialize<List<string>>(GalleryImagesJson) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public void SetGalleryImages(List<string> images)
        {
            GalleryImagesJson = JsonSerializer.Serialize(images);
        }

        public List<string> GetSliderImages()
        {
            if (string.IsNullOrEmpty(SliderImagesJson))
                return new List<string>();
            
            try
            {
                return JsonSerializer.Deserialize<List<string>>(SliderImagesJson) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public void SetSliderImages(List<string> images)
        {
            SliderImagesJson = JsonSerializer.Serialize(images);
        }

        public List<string> GetAmenities()
        {
            if (string.IsNullOrEmpty(AmenitiesJson))
                return new List<string>();
            
            try
            {
                return JsonSerializer.Deserialize<List<string>>(AmenitiesJson) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public void SetAmenities(List<string> amenities)
        {
            AmenitiesJson = JsonSerializer.Serialize(amenities);
        }

        public List<HotelRoom> GetRooms()
        {
            if (string.IsNullOrEmpty(RoomsJson))
                return new List<HotelRoom>();
            
            try
            {
                return JsonSerializer.Deserialize<List<HotelRoom>>(RoomsJson) ?? new List<HotelRoom>();
            }
            catch
            {
                return new List<HotelRoom>();
            }
        }

        public void SetRooms(List<HotelRoom> rooms)
        {
            RoomsJson = JsonSerializer.Serialize(rooms);
        }

        public List<HotelFacility> GetFacilities()
        {
            if (string.IsNullOrEmpty(FacilitiesJson))
                return new List<HotelFacility>();
            
            try
            {
                return JsonSerializer.Deserialize<List<HotelFacility>>(FacilitiesJson) ?? new List<HotelFacility>();
            }
            catch
            {
                return new List<HotelFacility>();
            }
        }

        public void SetFacilities(List<HotelFacility> facilities)
        {
            FacilitiesJson = JsonSerializer.Serialize(facilities);
        }

        public HotelSocial GetSocial()
        {
            return new HotelSocial
            {
                Facebook = Facebook,
                Instagram = Instagram,
                Twitter = Twitter,
                LinkedIn = LinkedIn,
                YouTube = YouTube
            };
        }

        public void SetSocial(HotelSocial social)
        {
            Facebook = social.Facebook;
            Instagram = social.Instagram;
            Twitter = social.Twitter;
            LinkedIn = social.LinkedIn;
            YouTube = social.YouTube;
        }

        public HotelMeta GetMeta()
        {
            return new HotelMeta
            {
                Title = MetaTitle ?? HotelName,
                Description = MetaDescription ?? Description,
                Keywords = MetaKeywords ?? ""
            };
        }

        public void SetMeta(HotelMeta meta)
        {
            MetaTitle = meta.Title;
            MetaDescription = meta.Description;
            MetaKeywords = meta.Keywords;
        }

        public HotelLocation GetLocation()
        {
            return new HotelLocation
            {
                Latitude = Latitude ?? 0,
                Longitude = Longitude ?? 0
            };
        }

        public void SetLocation(HotelLocation location)
        {
            Latitude = location.Latitude;
            Longitude = location.Longitude;
        }

        public ComprehensiveHotel ToComprehensiveHotel()
        {
            return new ComprehensiveHotel
            {
                Name = HotelName,
                Description = Description ?? "",
                Address = Address,
                Phone = Phone,
                Email = Email,
                Website = Website ?? "",
                LogoUrl = LogoUrl ?? "",
                SliderImages = GetSliderImages(),
                GalleryImages = GetGalleryImages(),
                Facilities = GetFacilities(),
                Amenities = GetAmenities(),
                Rooms = GetRooms(),
                Social = GetSocial(),
                Meta = GetMeta(),
                Location = GetLocation(),
                StarRating = StarRating,
                Currency = Currency,
                Language = Language
            };
        }
    }
} 