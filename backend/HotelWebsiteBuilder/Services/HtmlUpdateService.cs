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

            // Hotel name güncelle - tüm placeholder'ları değiştir
            if (!string.IsNullOrEmpty(websiteKeys.hotelname))
            {
                updatedHtml = updatedHtml.Replace("Otel Adı", websiteKeys.hotelname);
                updatedHtml = updatedHtml.Replace("{{HOTEL_NAME}}", websiteKeys.hotelname);
            }

            // Logo güncelle
            if (!string.IsNullOrEmpty(websiteKeys.logourl))
            {
                updatedHtml = updatedHtml.Replace("{{LOGO_URL}}", websiteKeys.logourl);
            }
            else
            {
                updatedHtml = updatedHtml.Replace("{{LOGO_URL}}", "https://via.placeholder.com/150x50");
            }

            // Phone güncelle
            if (!string.IsNullOrEmpty(websiteKeys.phone))
            {
                updatedHtml = updatedHtml.Replace("Telefon numarası", websiteKeys.phone);
                updatedHtml = updatedHtml.Replace("{{PHONE}}", websiteKeys.phone);
            }

            // Email güncelle
            if (!string.IsNullOrEmpty(websiteKeys.email))
            {
                updatedHtml = updatedHtml.Replace("E-posta adresi", websiteKeys.email);
                updatedHtml = updatedHtml.Replace("{{EMAIL}}", websiteKeys.email);
            }

            // Address güncelle
            if (!string.IsNullOrEmpty(websiteKeys.address))
            {
                updatedHtml = updatedHtml.Replace("Adres bilgisi", websiteKeys.address);
                updatedHtml = updatedHtml.Replace("{{ADDRESS}}", websiteKeys.address);
            }

            // Gallery images güncelle
            if (!string.IsNullOrEmpty(websiteKeys.galleryimage1))
            {
                updatedHtml = updatedHtml.Replace("{{GALLERY_IMAGE_1}}", websiteKeys.galleryimage1);
            }
            else
            {
                updatedHtml = updatedHtml.Replace("{{GALLERY_IMAGE_1}}", "https://via.placeholder.com/600x400");
            }

            if (!string.IsNullOrEmpty(websiteKeys.galleryimage2))
            {
                updatedHtml = updatedHtml.Replace("{{GALLERY_IMAGE_2}}", websiteKeys.galleryimage2);
            }
            else
            {
                updatedHtml = updatedHtml.Replace("{{GALLERY_IMAGE_2}}", "https://via.placeholder.com/400x300");
            }

            if (!string.IsNullOrEmpty(websiteKeys.galleryimage3))
            {
                updatedHtml = updatedHtml.Replace("{{GALLERY_IMAGE_3}}", websiteKeys.galleryimage3);
            }
            else
            {
                updatedHtml = updatedHtml.Replace("{{GALLERY_IMAGE_3}}", "https://via.placeholder.com/400x300");
            }

            if (!string.IsNullOrEmpty(websiteKeys.galleryimage4))
            {
                updatedHtml = updatedHtml.Replace("{{GALLERY_IMAGE_4}}", websiteKeys.galleryimage4);
            }
            else
            {
                updatedHtml = updatedHtml.Replace("{{GALLERY_IMAGE_4}}", "https://via.placeholder.com/400x300");
            }

            if (!string.IsNullOrEmpty(websiteKeys.galleryimage5))
            {
                updatedHtml = updatedHtml.Replace("{{GALLERY_IMAGE_5}}", websiteKeys.galleryimage5);
            }
            else
            {
                updatedHtml = updatedHtml.Replace("{{GALLERY_IMAGE_5}}", "https://via.placeholder.com/600x400");
            }

            // Social media links güncelle
            if (!string.IsNullOrEmpty(websiteKeys.facebook))
            {
                updatedHtml = updatedHtml.Replace("{{FACEBOOK}}", websiteKeys.facebook);
            }
            else
            {
                updatedHtml = updatedHtml.Replace("{{FACEBOOK}}", "#");
            }

            if (!string.IsNullOrEmpty(websiteKeys.instagram))
            {
                updatedHtml = updatedHtml.Replace("{{INSTAGRAM}}", websiteKeys.instagram);
            }
            else
            {
                updatedHtml = updatedHtml.Replace("{{INSTAGRAM}}", "#");
            }

            if (!string.IsNullOrEmpty(websiteKeys.twitter))
            {
                updatedHtml = updatedHtml.Replace("{{TWITTER}}", websiteKeys.twitter);
            }
            else
            {
                updatedHtml = updatedHtml.Replace("{{TWITTER}}", "#");
            }

            if (!string.IsNullOrEmpty(websiteKeys.website))
            {
                updatedHtml = updatedHtml.Replace("{{WEBSITE}}", websiteKeys.website);
            }

            // Description güncelle
            if (!string.IsNullOrEmpty(websiteKeys.description))
            {
                updatedHtml = updatedHtml.Replace("Otel açıklaması buraya gelecek", websiteKeys.description);
                updatedHtml = updatedHtml.Replace("{{DESCRIPTION}}", websiteKeys.description);
            }

            // Amenities güncelle
            if (!string.IsNullOrEmpty(websiteKeys.amenities))
            {
                updatedHtml = updatedHtml.Replace("Özellikler listesi buraya gelecek", websiteKeys.amenities);
                updatedHtml = updatedHtml.Replace("{{AMENITIES}}", websiteKeys.amenities);
            }

            // Room types güncelle
            if (!string.IsNullOrEmpty(websiteKeys.roomtypes))
            {
                updatedHtml = updatedHtml.Replace("{{ROOM_TYPES}}", websiteKeys.roomtypes);
            }

            // Pricing güncelle
            if (!string.IsNullOrEmpty(websiteKeys.pricing))
            {
                updatedHtml = updatedHtml.Replace("Fiyat bilgisi", websiteKeys.pricing);
                updatedHtml = updatedHtml.Replace("{{PRICING}}", websiteKeys.pricing);
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