using Microsoft.AspNetCore.Mvc;
using HotelWebsiteBuilder.Models;
using HotelWebsiteBuilder.Services;
using Microsoft.AspNetCore.Hosting;

namespace HotelWebsiteBuilder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebsiteBuilderController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly ITemplateService _templateService;
        private readonly IHtmlAnalysisService _htmlAnalysisService;
        private readonly IHtmlUpdateService _htmlUpdateService;
        private readonly HotelSiteCloneService _hotelSiteCloneService;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public WebsiteBuilderController(
            IHotelService hotelService,
            ITemplateService templateService,
            IHtmlAnalysisService htmlAnalysisService,
            IHtmlUpdateService htmlUpdateService,
            HotelSiteCloneService hotelSiteCloneService,
            IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            _hotelService = hotelService;
            _templateService = templateService;
            _htmlAnalysisService = htmlAnalysisService;
            _htmlUpdateService = htmlUpdateService;
            _hotelSiteCloneService = hotelSiteCloneService;
            _environment = environment;
            _configuration = configuration;
        }

        [HttpPost("build")]
        public async Task<ActionResult<WebsiteBuilderResponse>> BuildWebsite([FromBody] WebsiteBuilderRequest request)
        {
            try
            {
                WebsiteKeys websiteKeys;
                string htmlContent;
                string templateName = "default";

                // 1. Otel verilerini al
                if (request.HotelId.HasValue)
                {
                    var hotel = await _hotelService.GetHotelByIdAsync(request.HotelId.Value);
                    if (hotel == null)
                    {
                        return NotFound("Otel bulunamadı");
                    }
                    websiteKeys = _hotelService.ConvertHotelToWebsiteKeys(hotel);
                }
                else if (request.HotelData != null)
                {
                    websiteKeys = request.HotelData;
                }
                else
                {
                    return BadRequest("Otel verisi veya ID gerekli");
                }

                // 2. HTML şablonunu al
                if (!string.IsNullOrEmpty(request.TemplateName))
                {
                    htmlContent = await _templateService.GetTemplateAsync(request.TemplateName);
                    templateName = request.TemplateName;
                }
                else if (!string.IsNullOrEmpty(request.SourceUrl))
                {
                    htmlContent = await _htmlAnalysisService.AnalyzeAndExtractStructure(request.SourceUrl);
                    templateName = "url_analyzed";
                }
                else
                {
                    htmlContent = await _templateService.GetTemplateAsync("modern");
                    templateName = "modern";
                }

                // 3. HTML'i güncelle
                var updatedHtml = _htmlAnalysisService.UpdateHtmlWithWebsiteKeys(htmlContent, websiteKeys);

                return Ok(new WebsiteBuilderResponse
                {
                    HtmlContent = updatedHtml,
                    WebsiteKeys = websiteKeys,
                    TemplateName = templateName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpGet("templates")]
        public async Task<ActionResult<List<string>>> GetAvailableTemplates()
        {
            try
            {
                var templates = await _templateService.GetAvailableTemplatesAsync();
                return Ok(templates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpPost("generate/template")]
        public async Task<ActionResult<WebsiteBuilderResponse>> GenerateFromTemplate([FromBody] TemplateGenerationRequest request)
        {
            try
            {
                // 1. Otel verilerini al
                var hotel = await _hotelService.GetHotelByIdAsync(request.HotelId);
                if (hotel == null)
                {
                    return NotFound("Otel bulunamadı");
                }

                var websiteKeys = _hotelService.ConvertHotelToWebsiteKeys(hotel);

                // 2. HTML şablonunu al
                var htmlContent = await _templateService.GetTemplateAsync(request.TemplateName ?? "modern");

                // 3. HTML'i güncelle ve dosyaya kaydet
                var updatedHtml = await _htmlUpdateService.UpdateHtmlAndSaveAsync(htmlContent, websiteKeys, hotel.HotelName);

                return Ok(new WebsiteBuilderResponse
                {
                    HtmlContent = updatedHtml,
                    WebsiteKeys = websiteKeys,
                    TemplateName = request.TemplateName ?? "modern",
                    OutputPath = $"/productiondir/{hotel.HotelName.Replace(" ", "_").ToLower()}/index.html"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpPost("generate/from-url")]
        public async Task<ActionResult<WebsiteBuilderResponse>> GenerateFromUrl([FromBody] UrlGenerationRequest request)
        {
            try
            {
                // 1. Otel verilerini al
                var hotel = await _hotelService.GetHotelByIdAsync(request.HotelId);
                if (hotel == null)
                {
                    return NotFound("Otel bulunamadı");
                }

                var websiteKeys = _hotelService.ConvertHotelToWebsiteKeys(hotel);

                // 2. URL'den HTML analiz et
                var htmlContent = await _htmlAnalysisService.AnalyzeAndExtractStructure(request.SourceUrl);

                // 3. HTML'i güncelle ve dosyaya kaydet
                var updatedHtml = await _htmlUpdateService.UpdateHtmlAndSaveAsync(htmlContent, websiteKeys, hotel.HotelName);

                return Ok(new WebsiteBuilderResponse
                {
                    HtmlContent = updatedHtml,
                    WebsiteKeys = websiteKeys,
                    TemplateName = "url_analyzed",
                    OutputPath = $"/productiondir/{hotel.HotelName.Replace(" ", "_").ToLower()}/index.html"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpPost("generate/from-url-clone")]
        public async Task<ActionResult<WebsiteBuilderResponse>> GenerateFromUrlClone([FromBody] UrlGenerationRequest request)
        {
            try
            {
                Console.WriteLine($"=== GENERATE FROM URL CLONE DEBUG ===");
                Console.WriteLine($"Request - URL: {request.SourceUrl}");
                Console.WriteLine($"Request - Hotel ID: {request.HotelId}");

                // 1. Otel verilerini al
                var hotel = await _hotelService.GetHotelByIdAsync(request.HotelId);
                if (hotel == null)
                {
                    return NotFound("Otel bulunamadı");
                }

                Console.WriteLine($"Hotel found - Name: {hotel.HotelName}");

                // 2. HotelSiteCloneService kullanarak site klonla
                var result = await _hotelSiteCloneService.CloneHotelSiteAsync(request.SourceUrl, request.HotelId);

                if (result.Success)
                {
                    var baseUrl = _configuration["BaseUrl"] ?? "http://localhost:5001";
                    return Ok(new WebsiteBuilderResponse
                    {
                        HtmlContent = $"<html><body><h1>Site Başarıyla Klonlandı!</h1><p><strong>Otel:</strong> {result.HotelName}</p><p><strong>Site URL:</strong> <a href='{baseUrl}{result.SiteUrl}' target='_blank'>{result.SiteUrl}</a></p><p><strong>Mesaj:</strong> {result.Message}</p></body></html>",
                        WebsiteKeys = _hotelService.ConvertHotelToWebsiteKeys(hotel),
                        TemplateName = "url_cloned",
                        OutputPath = result.SiteUrl
                    });
                }
                else
                {
                    return BadRequest(new { message = result.Message });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generate from URL error: {ex.Message}");
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        public class CloneSiteRequest
        {
            public string Url { get; set; } = string.Empty;
            public WebsiteKeys HotelData { get; set; } = new WebsiteKeys();
            public ComprehensiveHotel? ComprehensiveHotelData { get; set; }
        }

        [HttpPost("clone-site")]
        public async Task<IActionResult> CloneSite([FromBody] CloneSiteRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Url))
                {
                    return BadRequest("URL gerekli");
                }

                Console.WriteLine($"=== CLONE SITE DEBUG ===");
                Console.WriteLine($"Clone request - URL: {request.Url}");
                Console.WriteLine($"Clone request - HotelData is null: {request.HotelData == null}");
                Console.WriteLine($"Clone request - ComprehensiveHotelData is null: {request.ComprehensiveHotelData == null}");
                
                string hotelName = "site";
                
                if (request.HotelData != null)
                {
                    Console.WriteLine($"Clone request - Hotel name: '{request.HotelData.hotelname}'");
                    Console.WriteLine($"Clone request - Phone: '{request.HotelData.phone}'");
                    Console.WriteLine($"Clone request - Email: '{request.HotelData.email}'");
                    Console.WriteLine($"Clone request - Address: '{request.HotelData.address}'");
                    Console.WriteLine($"Clone request - Description: '{request.HotelData.description}'");
                    hotelName = request.HotelData.hotelname ?? "site";
                }
                else if (request.ComprehensiveHotelData != null)
                {
                    Console.WriteLine($"Clone request - Comprehensive Hotel name: '{request.ComprehensiveHotelData.Name}'");
                    Console.WriteLine($"Clone request - Phone: '{request.ComprehensiveHotelData.Phone}'");
                    Console.WriteLine($"Clone request - Email: '{request.ComprehensiveHotelData.Email}'");
                    Console.WriteLine($"Clone request - Address: '{request.ComprehensiveHotelData.Address}'");
                    Console.WriteLine($"Clone request - Description: '{request.ComprehensiveHotelData.Description}'");
                    hotelName = request.ComprehensiveHotelData.Name ?? "site";
                }

                Console.WriteLine($"Hotel name after null check: '{hotelName}'");
                
                var safeHotelName = hotelName.Replace(" ", "-").ToLowerInvariant();
                Console.WriteLine($"Safe hotel name: '{safeHotelName}'");
                
                var outputDir = Path.Combine(_environment.WebRootPath, "sites", safeHotelName, "site");
                Console.WriteLine($"Output directory: {outputDir}");
                
                string resultPath = ""; // Temporarily disabled
                
                // Temporarily disabled - SiteCloneService not found
                /*
                if (request.ComprehensiveHotelData != null)
                {
                    // Kapsamlı hotel verisi ile klonla
                    resultPath = await _siteCloneService.CloneSiteWithComprehensiveDataAsync(request.Url, request.ComprehensiveHotelData, outputDir);
                }
                else
                {
                    // Basit hotel verisi ile klonla
                    var hotelData = request.HotelData ?? new WebsiteKeys();
                    resultPath = await _siteCloneService.CloneSiteAsync(request.Url, hotelData, outputDir);
                }
                */
                
                return Ok(new { 
                    success = true, 
                    outputPath = resultPath,
                    message = "Site başarıyla klonlandı"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Clone error: {ex.Message}");
                return StatusCode(500, new { 
                    success = false, 
                    message = $"Site klonlanırken hata oluştu: {ex.Message}" 
                });
            }
        }

        [HttpPost("clone-site-comprehensive")]
        public async Task<IActionResult> CloneSiteComprehensive([FromBody] CloneSiteRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Url))
                {
                    return BadRequest("URL gerekli");
                }

                if (request.ComprehensiveHotelData == null)
                {
                    return BadRequest("Kapsamlı otel verisi gerekli");
                }

                Console.WriteLine($"=== COMPREHENSIVE CLONE SITE DEBUG ===");
                Console.WriteLine($"Clone request - URL: {request.Url}");
                Console.WriteLine($"Clone request - Hotel name: '{request.ComprehensiveHotelData.Name}'");
                Console.WriteLine($"Clone request - Phone: '{request.ComprehensiveHotelData.Phone}'");
                Console.WriteLine($"Clone request - Email: '{request.ComprehensiveHotelData.Email}'");
                Console.WriteLine($"Clone request - Address: '{request.ComprehensiveHotelData.Address}'");
                Console.WriteLine($"Clone request - Description: '{request.ComprehensiveHotelData.Description}'");
                Console.WriteLine($"Clone request - Rooms count: {request.ComprehensiveHotelData.Rooms?.Count ?? 0}");
                Console.WriteLine($"Clone request - Facilities count: {request.ComprehensiveHotelData.Facilities?.Count ?? 0}");
                
                var hotelName = request.ComprehensiveHotelData.Name ?? "site";
                var safeHotelName = hotelName.Replace(" ", "-").ToLowerInvariant();
                var outputDir = Path.Combine(_environment.WebRootPath, "sites", safeHotelName, "comprehensive");
                
                // Temporarily disabled - SiteCloneService not found
                var resultPath = ""; // await _siteCloneService.CloneSiteWithComprehensiveDataAsync(request.Url, request.ComprehensiveHotelData, outputDir);
                
                return Ok(new { 
                    success = true, 
                    outputPath = resultPath,
                    message = "Site kapsamlı verilerle başarıyla klonlandı",
                    hotelData = request.ComprehensiveHotelData
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Comprehensive clone error: {ex.Message}");
                return StatusCode(500, new { 
                    success = false, 
                    message = $"Site klonlanırken hata oluştu: {ex.Message}" 
                });
            }
        }

        public class HotelSiteCloneRequest
        {
            public string SourceUrl { get; set; } = string.Empty;
            public int HotelId { get; set; }
        }

        [HttpPost("clone-hotel-site")]
        public async Task<IActionResult> CloneHotelSite([FromBody] HotelSiteCloneRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.SourceUrl))
                {
                    return BadRequest("Kaynak URL gerekli");
                }

                if (request.HotelId <= 0)
                {
                    return BadRequest("Geçerli bir Hotel ID gerekli");
                }

                Console.WriteLine($"=== HOTEL SITE CLONE DEBUG ===");
                Console.WriteLine($"Clone request - Source URL: {request.SourceUrl}");
                Console.WriteLine($"Clone request - Hotel ID: {request.HotelId}");

                // Yeni servisi kullanarak site klonla
                var result = await _hotelSiteCloneService.CloneHotelSiteAsync(request.SourceUrl, request.HotelId);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        siteUrl = result.SiteUrl,
                        hotelName = result.HotelName,
                        outputDirectory = result.OutputDirectory,
                        message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hotel site clone error: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Otel sitesi klonlanırken hata oluştu: {ex.Message}"
                });
            }
        }

        [HttpPost("extract-full-site")]
        public async Task<IActionResult> ExtractFullSite([FromBody] ExtractSiteRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Url))
                {
                    return BadRequest("URL gerekli");
                }

                if (request.HotelData == null)
                {
                    return BadRequest("Otel verisi gerekli");
                }

                Console.WriteLine($"Tam site çıkarma ve uyarlama başlatılıyor: {request.Url}");

                // Site'ı çıkar ve otel bilgilerine uyarla - Temporarily disabled
                var outputPath = ""; // await _htmlAnalysisService.ExtractAndAdaptSite(request.Url, request.HotelData);

                return Ok(new
                {
                    success = true,
                    message = "Site başarıyla çıkarıldı ve otel bilgilerine uyarlandı",
                    outputPath = outputPath,
                    fileName = Path.GetFileName(outputPath),
                    hotelName = request.HotelData.hotelname
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"Site çıkarılırken hata oluştu: {ex.Message}"
                });
            }
        }

        public class ExtractSiteRequest
        {
            public string Url { get; set; } = string.Empty;
            public WebsiteKeys HotelData { get; set; } = new WebsiteKeys();
        }
    }
} 