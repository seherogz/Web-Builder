using System.ComponentModel.DataAnnotations;

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
        
        public string? GalleryImage1 { get; set; }
        public string? GalleryImage2 { get; set; }
        public string? GalleryImage3 { get; set; }
        public string? GalleryImage4 { get; set; }
        public string? GalleryImage5 { get; set; }
        
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? Website { get; set; }
        
        public string? Description { get; set; }
        public string? Amenities { get; set; }
        public string? RoomTypes { get; set; }
        public string? Pricing { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 