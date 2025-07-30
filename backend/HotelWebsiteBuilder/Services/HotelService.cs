using HotelWebsiteBuilder.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWebsiteBuilder.Services
{
    public interface IHotelService
    {
        Task<Hotel?> GetHotelByIdAsync(int id);
        Task<Hotel?> GetHotelByNameAsync(string name);
        Task<Hotel> CreateHotelAsync(Hotel hotel);
        Task<Hotel?> UpdateHotelAsync(Hotel hotel);
        Task<bool> DeleteHotelAsync(int id);
        Task<List<Hotel>> GetAllHotelsAsync();
        WebsiteKeys ConvertHotelToWebsiteKeys(Hotel hotel);
        ComprehensiveHotel ConvertHotelToComprehensiveHotel(Hotel hotel);
        Hotel ConvertComprehensiveHotelToHotel(ComprehensiveHotel comprehensiveHotel);
    }

    public class HotelService : IHotelService
    {
        private readonly ApplicationDbContext _context;

        public HotelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            return await _context.Hotels.FindAsync(id);
        }

        public async Task<Hotel?> GetHotelByNameAsync(string name)
        {
            return await _context.Hotels
                .FirstOrDefaultAsync(h => h.HotelName.ToLower().Contains(name.ToLower()));
        }

        public async Task<Hotel> CreateHotelAsync(Hotel hotel)
        {
            hotel.CreatedAt = DateTime.UtcNow;
            hotel.UpdatedAt = DateTime.UtcNow;
            
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            
            return hotel;
        }

        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            return await _context.Hotels.ToListAsync();
        }

        public async Task<Hotel?> UpdateHotelAsync(Hotel hotel)
        {
            var existingHotel = await _context.Hotels.FindAsync(hotel.Id);
            if (existingHotel == null)
            {
                return null;
            }

            // Update basic fields
            existingHotel.HotelName = hotel.HotelName;
            existingHotel.LogoUrl = hotel.LogoUrl;
            existingHotel.Phone = hotel.Phone;
            existingHotel.Email = hotel.Email;
            existingHotel.Address = hotel.Address;
            existingHotel.Website = hotel.Website;
            existingHotel.CheckInTime = hotel.CheckInTime;
            existingHotel.CheckOutTime = hotel.CheckOutTime;
            existingHotel.StarRating = hotel.StarRating;
            existingHotel.Currency = hotel.Currency;
            existingHotel.Language = hotel.Language;
            
            // Update JSON fields
            existingHotel.GalleryImagesJson = hotel.GalleryImagesJson;
            existingHotel.SliderImagesJson = hotel.SliderImagesJson;
            existingHotel.AmenitiesJson = hotel.AmenitiesJson;
            existingHotel.RoomsJson = hotel.RoomsJson;
            existingHotel.FacilitiesJson = hotel.FacilitiesJson;
            
            // Update social media
            existingHotel.Facebook = hotel.Facebook;
            existingHotel.Instagram = hotel.Instagram;
            existingHotel.Twitter = hotel.Twitter;
            existingHotel.LinkedIn = hotel.LinkedIn;
            existingHotel.YouTube = hotel.YouTube;
            
            // Update meta information
            existingHotel.MetaTitle = hotel.MetaTitle;
            existingHotel.MetaDescription = hotel.MetaDescription;
            existingHotel.MetaKeywords = hotel.MetaKeywords;
            
            // Update location
            existingHotel.Latitude = hotel.Latitude;
            existingHotel.Longitude = hotel.Longitude;
            
            // Update basic info
            existingHotel.Description = hotel.Description;
            
            // Legacy fields for backward compatibility
            existingHotel.GalleryImage1 = hotel.GalleryImage1;
            existingHotel.GalleryImage2 = hotel.GalleryImage2;
            existingHotel.GalleryImage3 = hotel.GalleryImage3;
            existingHotel.GalleryImage4 = hotel.GalleryImage4;
            existingHotel.GalleryImage5 = hotel.GalleryImage5;
            existingHotel.RoomTypes = hotel.RoomTypes;
            existingHotel.Pricing = hotel.Pricing;
            
            existingHotel.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingHotel;
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return false;
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }

        public WebsiteKeys ConvertHotelToWebsiteKeys(Hotel hotel)
        {
            return new WebsiteKeys
            {
                hotelname = hotel.HotelName,
                logourl = hotel.LogoUrl,
                phone = hotel.Phone,
                email = hotel.Email,
                address = hotel.Address,
                galleryimage1 = hotel.GalleryImage1,
                galleryimage2 = hotel.GalleryImage2,
                galleryimage3 = hotel.GalleryImage3,
                galleryimage4 = hotel.GalleryImage4,
                galleryimage5 = hotel.GalleryImage5,
                facebook = hotel.Facebook,
                instagram = hotel.Instagram,
                twitter = hotel.Twitter,
                website = hotel.Website,
                description = hotel.Description,
                amenities = hotel.AmenitiesJson, // Use JSON amenities
                roomtypes = hotel.RoomTypes,
                pricing = hotel.Pricing
            };
        }

        public ComprehensiveHotel ConvertHotelToComprehensiveHotel(Hotel hotel)
        {
            return hotel.ToComprehensiveHotel();
        }

        public Hotel ConvertComprehensiveHotelToHotel(ComprehensiveHotel comprehensiveHotel)
        {
            var hotel = new Hotel
            {
                HotelName = comprehensiveHotel.Name,
                Description = comprehensiveHotel.Description,
                Address = comprehensiveHotel.Address,
                Phone = comprehensiveHotel.Phone,
                Email = comprehensiveHotel.Email,
                Website = comprehensiveHotel.Website,
                LogoUrl = comprehensiveHotel.LogoUrl,
                CheckInTime = comprehensiveHotel.CheckInTime,
                CheckOutTime = comprehensiveHotel.CheckOutTime,
                StarRating = comprehensiveHotel.StarRating,
                Currency = comprehensiveHotel.Currency,
                Language = comprehensiveHotel.Language,
                
                // Social media
                Facebook = comprehensiveHotel.Social?.Facebook,
                Instagram = comprehensiveHotel.Social?.Instagram,
                Twitter = comprehensiveHotel.Social?.Twitter,
                LinkedIn = comprehensiveHotel.Social?.LinkedIn,
                YouTube = comprehensiveHotel.Social?.YouTube,
                
                // Meta information
                MetaTitle = comprehensiveHotel.Meta?.Title,
                MetaDescription = comprehensiveHotel.Meta?.Description,
                MetaKeywords = comprehensiveHotel.Meta?.Keywords,
                
                // Location
                Latitude = comprehensiveHotel.Location?.Latitude,
                Longitude = comprehensiveHotel.Location?.Longitude,
                
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Set JSON fields
            hotel.SetSliderImages(comprehensiveHotel.SliderImages);
            hotel.SetGalleryImages(comprehensiveHotel.GalleryImages);
            hotel.SetAmenities(comprehensiveHotel.Amenities);
            hotel.SetRooms(comprehensiveHotel.Rooms);
            hotel.SetFacilities(comprehensiveHotel.Facilities);

            return hotel;
        }
    }
} 