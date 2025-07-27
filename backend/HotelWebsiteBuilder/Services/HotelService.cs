using HotelWebsiteBuilder.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelWebsiteBuilder.Services
{
    public interface IHotelService
    {
        Task<Hotel?> GetHotelByIdAsync(int id);
        Task<Hotel?> GetHotelByNameAsync(string name);
        Task<Hotel> CreateHotelAsync(Hotel hotel);
        Task<List<Hotel>> GetAllHotelsAsync();
        WebsiteKeys ConvertHotelToWebsiteKeys(Hotel hotel);
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
                amenities = hotel.Amenities,
                roomtypes = hotel.RoomTypes,
                pricing = hotel.Pricing
            };
        }
    }
} 