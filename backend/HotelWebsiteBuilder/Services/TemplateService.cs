using HotelWebsiteBuilder.Models;

namespace HotelWebsiteBuilder.Services
{
    public interface ITemplateService
    {
        Task<string> GetTemplateAsync(string templateName);
        Task<List<string>> GetAvailableTemplatesAsync();
        Task<string> LoadTemplateFromFileAsync(string templateName);
    }

    public class TemplateService : ITemplateService
    {
        private readonly IWebHostEnvironment _environment;

        public TemplateService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> GetTemplateAsync(string templateName)
        {
            var templatePath = Path.Combine(_environment.WebRootPath, "designs", $"{templateName}.html");
            
            if (File.Exists(templatePath))
            {
                return await File.ReadAllTextAsync(templatePath);
            }
            
            // Varsayılan şablon döndür
            return await LoadDefaultTemplateAsync();
        }

        public async Task<List<string>> GetAvailableTemplatesAsync()
        {
            var designsPath = Path.Combine(_environment.WebRootPath, "designs");
            
            if (!Directory.Exists(designsPath))
            {
                Directory.CreateDirectory(designsPath);
                await CreateDefaultTemplatesAsync();
            }
            
            var files = Directory.GetFiles(designsPath, "*.html");
            var templates = files.Select(Path.GetFileNameWithoutExtension)
                               .Where(x => !string.IsNullOrEmpty(x))
                               .Select(x => x!)
                               .ToList();
            return templates;
        }

        public async Task<string> LoadTemplateFromFileAsync(string templateName)
        {
            return await GetTemplateAsync(templateName);
        }

        private async Task<string> LoadDefaultTemplateAsync()
        {
            return await Task.FromResult(@"
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
</html>");
        }

        private async Task CreateDefaultTemplatesAsync()
        {
            var designsPath = Path.Combine(_environment.WebRootPath, "designs");
            
            // Modern template
            var modernTemplate = await LoadDefaultTemplateAsync();
            await File.WriteAllTextAsync(Path.Combine(designsPath, "modern.html"), modernTemplate);
            
            // Classic template
            var classicTemplate = modernTemplate.Replace("bootstrap@5.1.3", "bootstrap@4.6.0")
                                               .Replace("font-awesome/6.0.0", "font-awesome/5.15.4");
            await File.WriteAllTextAsync(Path.Combine(designsPath, "classic.html"), classicTemplate);
            
            // Luxury template
            var luxuryTemplate = modernTemplate.Replace("bg-dark", "bg-dark bg-gradient")
                                              .Replace("btn-primary", "btn-outline-light");
            await File.WriteAllTextAsync(Path.Combine(designsPath, "luxury.html"), luxuryTemplate);

            // Check if new templates already exist, if not create them
            var minimalistPath = Path.Combine(designsPath, "minimalist.html");
            var boutiquePath = Path.Combine(designsPath, "boutique.html");
            var resortPath = Path.Combine(designsPath, "resort.html");

            if (!File.Exists(minimalistPath))
            {
                var minimalistTemplate = await LoadMinimalistTemplateAsync();
                await File.WriteAllTextAsync(minimalistPath, minimalistTemplate);
            }

            if (!File.Exists(boutiquePath))
            {
                var boutiqueTemplate = await LoadBoutiqueTemplateAsync();
                await File.WriteAllTextAsync(boutiquePath, boutiqueTemplate);
            }

            if (!File.Exists(resortPath))
            {
                var resortTemplate = await LoadResortTemplateAsync();
                await File.WriteAllTextAsync(resortPath, resortTemplate);
            }
        }

        private async Task<string> LoadMinimalistTemplateAsync()
        {
            return await File.ReadAllTextAsync(Path.Combine(_environment.WebRootPath, "designs", "minimalist.html"));
        }

        private async Task<string> LoadBoutiqueTemplateAsync()
        {
            return await File.ReadAllTextAsync(Path.Combine(_environment.WebRootPath, "designs", "boutique.html"));
        }

        private async Task<string> LoadResortTemplateAsync()
        {
            return await File.ReadAllTextAsync(Path.Combine(_environment.WebRootPath, "designs", "resort.html"));
        }
    }
} 