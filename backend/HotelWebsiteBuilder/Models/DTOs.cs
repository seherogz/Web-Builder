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

    public class WebsiteBuilderRequest
    {
        public string? TemplateName { get; set; }
        public string? SourceUrl { get; set; }
        public int? HotelId { get; set; }
        public WebsiteKeys? HotelData { get; set; }
    }

    public class WebsiteBuilderResponse
    {
        public string HtmlContent { get; set; } = string.Empty;
        public WebsiteKeys WebsiteKeys { get; set; } = new();
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
} 