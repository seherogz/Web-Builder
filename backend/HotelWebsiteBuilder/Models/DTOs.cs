namespace HotelWebsiteBuilder.Models
{
    public class WebsiteKeys
    {
        public string? hotelname { get; set; }
        public string? logourl { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? address { get; set; }
        public string? galleryimage1 { get; set; }
        public string? galleryimage2 { get; set; }
        public string? galleryimage3 { get; set; }
        public string? galleryimage4 { get; set; }
        public string? galleryimage5 { get; set; }
        public string? facebook { get; set; }
        public string? instagram { get; set; }
        public string? twitter { get; set; }
        public string? website { get; set; }
        public string? description { get; set; }
        public string? amenities { get; set; }
        public string? roomtypes { get; set; }
        public string? pricing { get; set; }
    }

    // Yeni kapsamlı hotel modelleri
    public class HotelLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class HotelMeta
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
    }

    public class HotelSocial
    {
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? LinkedIn { get; set; }
        public string? YouTube { get; set; }
    }

    public class HotelRoom
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public List<string> Features { get; set; } = new List<string>();
        public List<string> Images { get; set; } = new List<string>();
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class HotelFacility
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
    }

    public class ComprehensiveHotel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        
        // Check-in/out times
        public string CheckInTime { get; set; } = "14:00";
        public string CheckOutTime { get; set; } = "12:00";
        
        // Images
        public List<string> SliderImages { get; set; } = new List<string>();
        public List<string> GalleryImages { get; set; } = new List<string>();
        
        // Facilities and amenities
        public List<HotelFacility> Facilities { get; set; } = new List<HotelFacility>();
        public List<string> Amenities { get; set; } = new List<string>();
        
        // Rooms
        public List<HotelRoom> Rooms { get; set; } = new List<HotelRoom>();
        
        // Social media
        public HotelSocial Social { get; set; } = new HotelSocial();
        
        // Meta information
        public HotelMeta Meta { get; set; } = new HotelMeta();
        
        // Location
        public HotelLocation Location { get; set; } = new HotelLocation();
        
        // Additional information
        public int StarRating { get; set; } = 4;
        public string Currency { get; set; } = "TRY";
        public string Language { get; set; } = "tr";
    }

    public class WebsiteBuilderRequest
    {
        public string? TemplateName { get; set; }
        public string? SourceUrl { get; set; }
        public int? HotelId { get; set; }
        public WebsiteKeys? HotelData { get; set; }
        public ComprehensiveHotel? ComprehensiveHotelData { get; set; }
    }

    public class WebsiteBuilderResponse
    {
        public string HtmlContent { get; set; } = string.Empty;
        public WebsiteKeys WebsiteKeys { get; set; } = new();
        public ComprehensiveHotel? ComprehensiveHotel { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string OutputPath { get; set; } = string.Empty;
    }

    public class HotelSearchRequest
    {
        public int? HotelId { get; set; }
        public string? HotelName { get; set; }
    }

    public class TemplateGenerationRequest
    {
        public int HotelId { get; set; }
        public string? TemplateName { get; set; }
    }

    public class UrlGenerationRequest
    {
        public int HotelId { get; set; }
        public string SourceUrl { get; set; } = string.Empty;
    }

    public class CloneSiteRequest
    {
        public string Url { get; set; } = string.Empty;
        public WebsiteKeys? HotelData { get; set; }
        public ComprehensiveHotel? ComprehensiveHotelData { get; set; }
    }
} 