using HotelWebsiteBuilder.Models;

namespace HotelWebsiteBuilder.Services
{
    public class HtmlUpdateService : IHtmlUpdateService
    {
        private readonly IWebHostEnvironment _environment;

        public HtmlUpdateService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UpdateHtmlAndSaveAsync(string htmlContent, WebsiteKeys websiteKeys, string hotelName)
        {
            // HTML'i güncelle
            var updatedHtml = UpdateHtmlWithWebsiteKeys(htmlContent, websiteKeys);
            
            // Dosyaya kaydet
            var outputPath = await SaveHtmlToFileAsync(updatedHtml, hotelName);
            
            return updatedHtml;
        }

        public string UpdateHtmlWithWebsiteKeys(string htmlContent, WebsiteKeys websiteKeys)
        {
            var updatedHtml = htmlContent;

            // Title güncelle
            if (!string.IsNullOrEmpty(websiteKeys.hotelname))
            {
                updatedHtml = updatedHtml.UpdateTitle(websiteKeys.hotelname);
            }

            // Meta description güncelle
            if (!string.IsNullOrEmpty(websiteKeys.description))
            {
                updatedHtml = updatedHtml.UpdateMetaDescription(websiteKeys.description);
            }

            // Hotel name güncelle
            if (!string.IsNullOrEmpty(websiteKeys.hotelname))
            {
                updatedHtml = updatedHtml.UpdateElementById("hotel-name", websiteKeys.hotelname);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{HOTEL_NAME}}", websiteKeys.hotelname);
            }

            // Logo güncelle
            if (!string.IsNullOrEmpty(websiteKeys.logourl))
            {
                updatedHtml = updatedHtml.UpdateImageSrc("hotel-logo", websiteKeys.logourl);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{LOGO_URL}}", websiteKeys.logourl);
            }
            else
            {
                // Logo yoksa placeholder kullan
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{LOGO_URL}}", "https://via.placeholder.com/150x50");
            }

            // Phone güncelle
            if (!string.IsNullOrEmpty(websiteKeys.phone))
            {
                updatedHtml = updatedHtml.UpdateElementById("hotel-phone", websiteKeys.phone);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{PHONE}}", websiteKeys.phone);
            }

            // Email güncelle
            if (!string.IsNullOrEmpty(websiteKeys.email))
            {
                updatedHtml = updatedHtml.UpdateElementById("hotel-email", websiteKeys.email);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{EMAIL}}", websiteKeys.email);
            }

            // Address güncelle
            if (!string.IsNullOrEmpty(websiteKeys.address))
            {
                updatedHtml = updatedHtml.UpdateElementById("hotel-address", websiteKeys.address);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{ADDRESS}}", websiteKeys.address);
            }

            // Gallery images güncelle
            if (!string.IsNullOrEmpty(websiteKeys.galleryimage1))
            {
                updatedHtml = updatedHtml.UpdateImageSrc("gallery-image-1", websiteKeys.galleryimage1);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{GALLERY_IMAGE_1}}", websiteKeys.galleryimage1);
            }
            else
            {
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{GALLERY_IMAGE_1}}", "https://via.placeholder.com/600x400");
            }

            if (!string.IsNullOrEmpty(websiteKeys.galleryimage2))
            {
                updatedHtml = updatedHtml.UpdateImageSrc("gallery-image-2", websiteKeys.galleryimage2);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{GALLERY_IMAGE_2}}", websiteKeys.galleryimage2);
            }
            else
            {
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{GALLERY_IMAGE_2}}", "https://via.placeholder.com/400x300");
            }

            if (!string.IsNullOrEmpty(websiteKeys.galleryimage3))
            {
                updatedHtml = updatedHtml.UpdateImageSrc("gallery-image-3", websiteKeys.galleryimage3);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{GALLERY_IMAGE_3}}", websiteKeys.galleryimage3);
            }
            else
            {
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{GALLERY_IMAGE_3}}", "https://via.placeholder.com/400x300");
            }

            if (!string.IsNullOrEmpty(websiteKeys.galleryimage4))
            {
                updatedHtml = updatedHtml.UpdateImageSrc("gallery-image-4", websiteKeys.galleryimage4);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{GALLERY_IMAGE_4}}", websiteKeys.galleryimage4);
            }
            else
            {
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{GALLERY_IMAGE_4}}", "https://via.placeholder.com/400x300");
            }

            if (!string.IsNullOrEmpty(websiteKeys.galleryimage5))
            {
                updatedHtml = updatedHtml.UpdateImageSrc("gallery-image-5", websiteKeys.galleryimage5);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{GALLERY_IMAGE_5}}", websiteKeys.galleryimage5);
            }
            else
            {
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{GALLERY_IMAGE_5}}", "https://via.placeholder.com/600x400");
            }

            // Social media links güncelle
            if (!string.IsNullOrEmpty(websiteKeys.facebook))
            {
                updatedHtml = updatedHtml.UpdateLinkHref("facebook-link", websiteKeys.facebook);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{FACEBOOK}}", websiteKeys.facebook);
            }
            else
            {
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{FACEBOOK}}", "#");
            }

            if (!string.IsNullOrEmpty(websiteKeys.instagram))
            {
                updatedHtml = updatedHtml.UpdateLinkHref("instagram-link", websiteKeys.instagram);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{INSTAGRAM}}", websiteKeys.instagram);
            }
            else
            {
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{INSTAGRAM}}", "#");
            }

            if (!string.IsNullOrEmpty(websiteKeys.twitter))
            {
                updatedHtml = updatedHtml.UpdateLinkHref("twitter-link", websiteKeys.twitter);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{TWITTER}}", websiteKeys.twitter);
            }
            else
            {
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{TWITTER}}", "#");
            }

            if (!string.IsNullOrEmpty(websiteKeys.website))
            {
                updatedHtml = updatedHtml.UpdateLinkHref("website-link", websiteKeys.website);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{WEBSITE}}", websiteKeys.website);
            }

            // Description güncelle
            if (!string.IsNullOrEmpty(websiteKeys.description))
            {
                updatedHtml = updatedHtml.UpdateElementById("hotel-description", websiteKeys.description);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{DESCRIPTION}}", websiteKeys.description);
            }

            // Amenities güncelle
            if (!string.IsNullOrEmpty(websiteKeys.amenities))
            {
                updatedHtml = updatedHtml.UpdateElementById("hotel-amenities", websiteKeys.amenities);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{AMENITIES}}", websiteKeys.amenities);
            }

            // Room types güncelle
            if (!string.IsNullOrEmpty(websiteKeys.roomtypes))
            {
                updatedHtml = updatedHtml.UpdateElementById("hotel-room-types", websiteKeys.roomtypes);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{ROOM_TYPES}}", websiteKeys.roomtypes);
            }

            // Pricing güncelle
            if (!string.IsNullOrEmpty(websiteKeys.pricing))
            {
                updatedHtml = updatedHtml.UpdateElementById("hotel-pricing", websiteKeys.pricing);
                updatedHtml = updatedHtml.UpdatePlaceholderText("{{PRICING}}", websiteKeys.pricing);
            }

            return updatedHtml;
        }

        public async Task<string> SaveHtmlToFileAsync(string htmlContent, string hotelName)
        {
            try
            {
                // wwwroot/productiondir dizinini oluştur
                var productionDir = Path.Combine(_environment.WebRootPath, "productiondir");
                if (!Directory.Exists(productionDir))
                {
                    Directory.CreateDirectory(productionDir);
                }

                // Hotel adına göre alt dizin oluştur
                var hotelDirName = hotelName.Replace(" ", "_").ToLower();
                var hotelDir = Path.Combine(productionDir, hotelDirName);
                if (!Directory.Exists(hotelDir))
                {
                    Directory.CreateDirectory(hotelDir);
                }

                // index.html dosyasını oluştur
                var filePath = Path.Combine(hotelDir, "index.html");
                await File.WriteAllTextAsync(filePath, htmlContent);

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"HTML dosyası kaydedilirken hata oluştu: {ex.Message}");
            }
        }
    }
} 