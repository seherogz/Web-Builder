using Microsoft.AspNetCore.Mvc;
using HotelWebsiteBuilder.Models;
using HotelWebsiteBuilder.Services;

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

        public WebsiteBuilderController(
            IHotelService hotelService,
            ITemplateService templateService,
            IHtmlAnalysisService htmlAnalysisService,
            IHtmlUpdateService htmlUpdateService)
        {
            _hotelService = hotelService;
            _templateService = templateService;
            _htmlAnalysisService = htmlAnalysisService;
            _htmlUpdateService = htmlUpdateService;
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
    }
} 