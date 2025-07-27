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

        public WebsiteBuilderController(
            IHotelService hotelService,
            ITemplateService templateService,
            IHtmlAnalysisService htmlAnalysisService)
        {
            _hotelService = hotelService;
            _templateService = templateService;
            _htmlAnalysisService = htmlAnalysisService;
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

        [HttpGet("hotels")]
        public async Task<ActionResult<List<Hotel>>> GetAllHotels()
        {
            try
            {
                var hotels = await _hotelService.GetAllHotelsAsync();
                return Ok(hotels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpGet("hotels/{id}")]
        public async Task<ActionResult<Hotel>> GetHotelById(int id)
        {
            try
            {
                var hotel = await _hotelService.GetHotelByIdAsync(id);
                if (hotel == null)
                {
                    return NotFound("Otel bulunamadı");
                }
                return Ok(hotel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpPost("hotels")]
        public async Task<ActionResult<Hotel>> CreateHotel([FromBody] Hotel hotel)
        {
            try
            {
                var createdHotel = await _hotelService.CreateHotelAsync(hotel);
                return CreatedAtAction(nameof(GetHotelById), new { id = createdHotel.Id }, createdHotel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpPost("hotels/search")]
        public async Task<ActionResult<Hotel>> SearchHotel([FromBody] HotelSearchRequest request)
        {
            try
            {
                Hotel? hotel = null;
                
                if (request.HotelId.HasValue)
                {
                    hotel = await _hotelService.GetHotelByIdAsync(request.HotelId.Value);
                }
                else if (!string.IsNullOrEmpty(request.HotelName))
                {
                    hotel = await _hotelService.GetHotelByNameAsync(request.HotelName);
                }

                if (hotel == null)
                {
                    return NotFound("Otel bulunamadı");
                }

                return Ok(hotel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
} 