using HtmlAgilityPack;
using HotelWebsiteBuilder.Models;

namespace HotelWebsiteBuilder.Services
{
    public interface IHtmlAnalysisService
    {
        Task<string> AnalyzeAndExtractStructure(string url);
        Task<WebsiteKeys?> AnalyzeAndExtractHotelData(string url);
        string UpdateHtmlWithWebsiteKeys(string htmlContent, WebsiteKeys websiteKeys);
    }

    public class HtmlAnalysisService : IHtmlAnalysisService
    {
        private readonly HttpClient _httpClient;

        public HtmlAnalysisService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AnalyzeAndExtractStructure(string url)
        {
            try
            {
                Console.WriteLine($"URL'den HTML alınıyor: {url}");
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                var htmlContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"HTML alındı, uzunluk: {htmlContent.Length}");
                
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // URL'den gelen gerçek HTML'i döndür, default template değil
                return htmlContent;
            }
            catch (Exception ex)
            {
                // Hata durumunda varsayılan şablon döndür
                Console.WriteLine($"URL'den HTML alınamadı: {ex.Message}");
                return CreateDefaultHotelTemplate();
            }
        }

        public async Task<WebsiteKeys?> AnalyzeAndExtractHotelData(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();
                
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                var websiteKeys = new WebsiteKeys();

                // Otel adını çıkar
                var titleElement = doc.DocumentNode.SelectSingleNode("//title");
                if (titleElement != null)
                {
                    var title = titleElement.InnerText.Trim();
                    if (title.Contains("Hotel") || title.Contains("Otel") || title.Contains("Resort"))
                    {
                        websiteKeys.hotelname = title;
                    }
                }

                // H1 etiketlerinden otel adını çıkar
                var h1Elements = doc.DocumentNode.SelectNodes("//h1");
                if (h1Elements != null)
                {
                    foreach (var h1 in h1Elements)
                    {
                        var text = h1.InnerText.Trim();
                        if (text.Contains("Hotel") || text.Contains("Otel") || text.Contains("Resort"))
                        {
                            websiteKeys.hotelname = text;
                            break;
                        }
                    }
                }

                // Telefon numarasını çıkar
                var phoneElements = doc.DocumentNode.SelectNodes("//a[contains(@href, 'tel:')]");
                if (phoneElements != null)
                {
                    foreach (var phone in phoneElements)
                    {
                        var href = phone.GetAttributeValue("href", "");
                        if (href.StartsWith("tel:"))
                        {
                            websiteKeys.phone = href.Replace("tel:", "");
                            break;
                        }
                    }
                }

                // E-posta adresini çıkar
                var emailElements = doc.DocumentNode.SelectNodes("//a[contains(@href, 'mailto:')]");
                if (emailElements != null)
                {
                    foreach (var email in emailElements)
                    {
                        var href = email.GetAttributeValue("href", "");
                        if (href.StartsWith("mailto:"))
                        {
                            websiteKeys.email = href.Replace("mailto:", "");
                            break;
                        }
                    }
                }

                // Adres bilgisini çıkar
                var addressElements = doc.DocumentNode.SelectNodes("//address");
                if (addressElements != null)
                {
                    foreach (var address in addressElements)
                    {
                        var addressText = address.InnerText.Trim();
                        if (!string.IsNullOrEmpty(addressText))
                        {
                            websiteKeys.address = addressText;
                            break;
                        }
                    }
                }

                // Logo URL'sini çıkar
                var logoElements = doc.DocumentNode.SelectNodes("//img[contains(@class, 'logo') or contains(@alt, 'logo') or contains(@alt, 'Logo')]");
                if (logoElements != null)
                {
                    foreach (var logo in logoElements)
                    {
                        var src = logo.GetAttributeValue("src", "");
                        if (!string.IsNullOrEmpty(src))
                        {
                            websiteKeys.logourl = src;
                            break;
                        }
                    }
                }

                // Galeri resimlerini çıkar
                var galleryElements = doc.DocumentNode.SelectNodes("//img[contains(@class, 'gallery') or contains(@class, 'slider') or contains(@alt, 'hotel')]");
                if (galleryElements != null)
                {
                    var imageUrls = new List<string>();
                    foreach (var img in galleryElements.Take(5))
                    {
                        var src = img.GetAttributeValue("src", "");
                        if (!string.IsNullOrEmpty(src))
                        {
                            imageUrls.Add(src);
                        }
                    }

                    if (imageUrls.Count > 0) websiteKeys.galleryimage1 = imageUrls[0];
                    if (imageUrls.Count > 1) websiteKeys.galleryimage2 = imageUrls[1];
                    if (imageUrls.Count > 2) websiteKeys.galleryimage3 = imageUrls[2];
                    if (imageUrls.Count > 3) websiteKeys.galleryimage4 = imageUrls[3];
                    if (imageUrls.Count > 4) websiteKeys.galleryimage5 = imageUrls[4];
                }

                // Sosyal medya linklerini çıkar
                var socialElements = doc.DocumentNode.SelectNodes("//a[contains(@href, 'facebook.com') or contains(@href, 'instagram.com') or contains(@href, 'twitter.com')]");
                if (socialElements != null)
                {
                    foreach (var social in socialElements)
                    {
                        var href = social.GetAttributeValue("href", "");
                        if (href.Contains("facebook.com"))
                            websiteKeys.facebook = href;
                        else if (href.Contains("instagram.com"))
                            websiteKeys.instagram = href;
                        else if (href.Contains("twitter.com"))
                            websiteKeys.twitter = href;
                    }
                }

                // Açıklama metnini çıkar
                var descriptionElements = doc.DocumentNode.SelectNodes("//p[contains(@class, 'description') or contains(@class, 'about')]");
                if (descriptionElements != null)
                {
                    foreach (var desc in descriptionElements)
                    {
                        var text = desc.InnerText.Trim();
                        if (text.Length > 50 && text.Length < 500)
                        {
                            websiteKeys.description = text;
                            break;
                        }
                    }
                }

                return websiteKeys;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string UpdateHtmlWithWebsiteKeys(string htmlContent, WebsiteKeys websiteKeys)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            // WebsiteKeys objesindeki tüm özellikleri kontrol et
            var properties = typeof(WebsiteKeys).GetProperties();
            
            foreach (var property in properties)
            {
                var value = property.GetValue(websiteKeys)?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    var elementId = property.Name.ToLower();
                    
                    // ID ile element bul
                    var element = doc.GetElementbyId(elementId);
                    if (element != null)
                    {
                        UpdateElement(element, value);
                    }
                    
                    // Class ile element bul (alternatif)
                    var elementsByClass = doc.DocumentNode.SelectNodes($"//*[contains(@class, '{elementId}')]");
                    if (elementsByClass != null)
                    {
                        foreach (var el in elementsByClass)
                        {
                            UpdateElement(el, value);
                        }
                    }
                }
            }

            // Placeholder metinleri de güncelle
            var updatedHtml = doc.DocumentNode.OuterHtml;
            
            if (!string.IsNullOrEmpty(websiteKeys.hotelname))
            {
                updatedHtml = updatedHtml.Replace("Otel Adı", websiteKeys.hotelname);
                updatedHtml = updatedHtml.Replace("{{HOTEL_NAME}}", websiteKeys.hotelname);
            }
            
            if (!string.IsNullOrEmpty(websiteKeys.phone))
            {
                updatedHtml = updatedHtml.Replace("Telefon numarası", websiteKeys.phone);
                updatedHtml = updatedHtml.Replace("{{PHONE}}", websiteKeys.phone);
            }
            
            if (!string.IsNullOrEmpty(websiteKeys.email))
            {
                updatedHtml = updatedHtml.Replace("E-posta adresi", websiteKeys.email);
                updatedHtml = updatedHtml.Replace("{{EMAIL}}", websiteKeys.email);
            }
            
            if (!string.IsNullOrEmpty(websiteKeys.address))
            {
                updatedHtml = updatedHtml.Replace("Adres bilgisi", websiteKeys.address);
                updatedHtml = updatedHtml.Replace("{{ADDRESS}}", websiteKeys.address);
            }
            
            if (!string.IsNullOrEmpty(websiteKeys.description))
            {
                updatedHtml = updatedHtml.Replace("Otel açıklaması buraya gelecek", websiteKeys.description);
                updatedHtml = updatedHtml.Replace("{{DESCRIPTION}}", websiteKeys.description);
            }
            
            if (!string.IsNullOrEmpty(websiteKeys.amenities))
            {
                updatedHtml = updatedHtml.Replace("Özellikler listesi buraya gelecek", websiteKeys.amenities);
                updatedHtml = updatedHtml.Replace("{{AMENITIES}}", websiteKeys.amenities);
            }
            
            if (!string.IsNullOrEmpty(websiteKeys.pricing))
            {
                updatedHtml = updatedHtml.Replace("Fiyat bilgisi", websiteKeys.pricing);
                updatedHtml = updatedHtml.Replace("{{PRICING}}", websiteKeys.pricing);
            }

            return updatedHtml;
        }

        private void UpdateElement(HtmlNode element, string value)
        {
            switch (element.Name.ToLower())
            {
                case "img":
                    element.SetAttributeValue("src", value);
                    break;
                case "a":
                    element.SetAttributeValue("href", value);
                    break;
                case "button":
                    element.SetAttributeValue("onclick", value);
                    break;
                default:
                    element.InnerHtml = value;
                    break;
            }
        }

        private string CreateHotelTemplateStructure()
        {
            return @"
<!DOCTYPE html>
<html lang=""tr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title id=""hotelname"">Otel Adı</title>
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css"" rel=""stylesheet"">
    <link href=""https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css"" rel=""stylesheet"">
    <style>
        .hero-section { background: linear-gradient(rgba(0,0,0,0.5), rgba(0,0,0,0.5)), url('https://via.placeholder.com/1920x1080'); background-size: cover; background-position: center; height: 100vh; }
        .room-card { transition: transform 0.3s; }
        .room-card:hover { transform: translateY(-5px); }
        .contact-info { background: #f8f9fa; }
    </style>
</head>
<body>
    <!-- Header -->
    <nav class=""navbar navbar-expand-lg navbar-dark bg-dark fixed-top"">
        <div class=""container"">
            <a class=""navbar-brand"" href=""#"">
                <img id=""logourl"" src=""https://via.placeholder.com/150x50"" alt=""Logo"" height=""50"">
            </a>
            <button class=""navbar-toggler"" type=""button"" data-bs-toggle=""collapse"" data-bs-target=""#navbarNav"">
                <span class=""navbar-toggler-icon""></span>
            </button>
            <div class=""collapse navbar-collapse"" id=""navbarNav"">
                <ul class=""navbar-nav ms-auto"">
                    <li class=""nav-item""><a class=""nav-link"" href=""#home"">Ana Sayfa</a></li>
                    <li class=""nav-item""><a class=""nav-link"" href=""#rooms"">Odalar</a></li>
                    <li class=""nav-item""><a class=""nav-link"" href=""#amenities"">Özellikler</a></li>
                    <li class=""nav-item""><a class=""nav-link"" href=""#contact"">İletişim</a></li>
                </ul>
            </div>
        </div>
    </nav>

    <!-- Hero Section -->
    <section id=""home"" class=""hero-section d-flex align-items-center"">
        <div class=""container text-center text-white"">
            <h1 class=""display-4 fw-bold mb-4"" id=""hotelname"">Otel Adı</h1>
            <p class=""lead mb-4"" id=""description"">Otel açıklaması buraya gelecek</p>
            <a href=""#rooms"" class=""btn btn-primary btn-lg"">Odaları İncele</a>
        </div>
    </section>

    <!-- About Section -->
    <section class=""py-5"">
        <div class=""container"">
            <div class=""row"">
                <div class=""col-lg-6"">
                    <h2 class=""mb-4"">Hakkımızda</h2>
                    <p id=""description"">Otel açıklaması buraya gelecek</p>
                    <div class=""row mt-4"">
                        <div class=""col-6"">
                            <i class=""fas fa-wifi text-primary mb-2""></i>
                            <h5>Ücretsiz Wi-Fi</h5>
                        </div>
                        <div class=""col-6"">
                            <i class=""fas fa-swimming-pool text-primary mb-2""></i>
                            <h5>Havuz</h5>
                        </div>
                    </div>
                </div>
                <div class=""col-lg-6"">
                    <img id=""galleryimage1"" src=""https://via.placeholder.com/600x400"" alt=""Otel"" class=""img-fluid rounded"">
                </div>
            </div>
        </div>
    </section>

    <!-- Rooms Section -->
    <section id=""rooms"" class=""py-5 bg-light"">
        <div class=""container"">
            <h2 class=""text-center mb-5"">Odalarımız</h2>
            <div class=""row"">
                <div class=""col-lg-4 mb-4"">
                    <div class=""card room-card"">
                        <img id=""galleryimage2"" src=""https://via.placeholder.com/400x300"" class=""card-img-top"" alt=""Standart Oda"">
                        <div class=""card-body"">
                            <h5 class=""card-title"">Standart Oda</h5>
                            <p class=""card-text"">Konforlu ve şık standart odalarımız</p>
                            <p class=""text-primary fw-bold"" id=""pricing"">Fiyat bilgisi</p>
                        </div>
                    </div>
                </div>
                <div class=""col-lg-4 mb-4"">
                    <div class=""card room-card"">
                        <img id=""galleryimage3"" src=""https://via.placeholder.com/400x300"" class=""card-img-top"" alt=""Deluxe Oda"">
                        <div class=""card-body"">
                            <h5 class=""card-title"">Deluxe Oda</h5>
                            <p class=""card-text"">Lüks ve geniş deluxe odalarımız</p>
                            <p class=""text-primary fw-bold"" id=""pricing"">Fiyat bilgisi</p>
                        </div>
                    </div>
                </div>
                <div class=""col-lg-4 mb-4"">
                    <div class=""card room-card"">
                        <img id=""galleryimage4"" src=""https://via.placeholder.com/400x300"" class=""card-img-top"" alt=""Süit"">
                        <div class=""card-body"">
                            <h5 class=""card-title"">Süit</h5>
                            <p class=""card-text"">En üst düzey konfor süit odalarımız</p>
                            <p class=""text-primary fw-bold"" id=""pricing"">Fiyat bilgisi</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Amenities Section -->
    <section id=""amenities"" class=""py-5"">
        <div class=""container"">
            <h2 class=""text-center mb-5"">Özelliklerimiz</h2>
            <div class=""row"">
                <div class=""col-lg-3 col-md-6 mb-4 text-center"">
                    <i class=""fas fa-wifi fa-3x text-primary mb-3""></i>
                    <h5>Ücretsiz Wi-Fi</h5>
                </div>
                <div class=""col-lg-3 col-md-6 mb-4 text-center"">
                    <i class=""fas fa-swimming-pool fa-3x text-primary mb-3""></i>
                    <h5>Havuz</h5>
                </div>
                <div class=""col-lg-3 col-md-6 mb-4 text-center"">
                    <i class=""fas fa-utensils fa-3x text-primary mb-3""></i>
                    <h5>Restoran</h5>
                </div>
                <div class=""col-lg-3 col-md-6 mb-4 text-center"">
                    <i class=""fas fa-spa fa-3x text-primary mb-3""></i>
                    <h5>Spa</h5>
                </div>
            </div>
            <div class=""text-center mt-4"">
                <p id=""amenities"">Özellikler listesi buraya gelecek</p>
            </div>
        </div>
    </section>

    <!-- Contact Section -->
    <section id=""contact"" class=""py-5 contact-info"">
        <div class=""container"">
            <h2 class=""text-center mb-5"">İletişim</h2>
            <div class=""row"">
                <div class=""col-lg-6"">
                    <h4>İletişim Bilgileri</h4>
                    <p><i class=""fas fa-map-marker-alt me-2""></i> <span id=""address"">Adres bilgisi</span></p>
                    <p><i class=""fas fa-phone me-2""></i> <a href=""tel:"" id=""phone"">Telefon numarası</a></p>
                    <p><i class=""fas fa-envelope me-2""></i> <a href=""mailto:"" id=""email"">E-posta adresi</a></p>
                    
                    <h5 class=""mt-4"">Sosyal Medya</h5>
                    <div class=""d-flex gap-3"">
                        <a href=""#"" id=""facebook"" class=""text-primary""><i class=""fab fa-facebook fa-2x""></i></a>
                        <a href=""#"" id=""instagram"" class=""text-primary""><i class=""fab fa-instagram fa-2x""></i></a>
                        <a href=""#"" id=""twitter"" class=""text-primary""><i class=""fab fa-twitter fa-2x""></i></a>
                    </div>
                </div>
                <div class=""col-lg-6"">
                    <img id=""galleryimage5"" src=""https://via.placeholder.com/600x400"" alt=""İletişim"" class=""img-fluid rounded"">
                </div>
            </div>
        </div>
    </section>

    <!-- Footer -->
    <footer class=""bg-dark text-white py-4"">
        <div class=""container text-center"">
            <p>&copy; 2024 <span id=""hotelname"">Otel Adı</span>. Tüm hakları saklıdır.</p>
        </div>
    </footer>

    <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js""></script>
</body>
</html>";
        }

        private string CreateDefaultHotelTemplate()
        {
            return CreateHotelTemplateStructure();
        }
    }
} 