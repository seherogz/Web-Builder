using HotelWebsiteBuilder.Models;

namespace HotelWebsiteBuilder.Services
{
    public interface IHtmlUpdateService
    {
        Task<string> UpdateHtmlAndSaveAsync(string htmlContent, WebsiteKeys websiteKeys, string hotelName);
        string UpdateHtmlWithWebsiteKeys(string htmlContent, WebsiteKeys websiteKeys);
        Task<string> SaveHtmlToFileAsync(string htmlContent, string hotelName);
    }
} 