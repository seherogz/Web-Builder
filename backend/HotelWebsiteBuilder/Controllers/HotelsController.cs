using Microsoft.AspNetCore.Mvc;
using HotelWebsiteBuilder.Models;
using HotelWebsiteBuilder.Services;

namespace HotelWebsiteBuilder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
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

        [HttpGet("{id}")]
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

        [HttpPost]
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

        [HttpPut("{id}")]
        public async Task<ActionResult<Hotel>> UpdateHotel(int id, [FromBody] Hotel hotel)
        {
            try
            {
                if (id != hotel.Id)
                {
                    return BadRequest("ID uyuşmazlığı");
                }

                var updatedHotel = await _hotelService.UpdateHotelAsync(hotel);
                if (updatedHotel == null)
                {
                    return NotFound("Otel bulunamadı");
                }

                return Ok(updatedHotel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHotel(int id)
        {
            try
            {
                var result = await _hotelService.DeleteHotelAsync(id);
                if (!result)
                {
                    return NotFound("Otel bulunamadı");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
} 