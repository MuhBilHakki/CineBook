using CineBook.DTOs;
using CineBook.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CineBook.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet] // GET /api/bookings
        public async Task<IActionResult> GetBookings([FromQuery] string? status, [FromQuery] DateTime? date)
        {
            var bookings = await _bookingService.GetAllAsync(status, date);
            var response = bookings.Select(_bookingService.ToDetailResponse).ToList();
            return Ok(response);
        }

        [HttpGet("{id:long}")] // GET /api/bookings/{id}
        public async Task<IActionResult> GetBooking(long id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null) return NotFound();
            return Ok(_bookingService.ToDetailResponse(booking));
        }

        [HttpPost] // POST /api/bookings
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            var result = await _bookingService.CreateAsync(request);

            if (result == null)
                return Conflict(new { error = "Kursi sudah dipesan oleh pengguna lain" });

            return Created($"/api/bookings/{result.Id}", result);
        }

        [HttpPut("{id:long}")] // PUT /api/bookings/{id}
        public async Task<IActionResult> UpdateBooking(long id, [FromBody] CreateBookingRequest request)
        {
            var result = await _bookingService.UpdateAsync(id, request);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id:long}")] // DELETE /api/bookings/{id}
        public async Task<IActionResult> CancelBooking(long id)
        {
            var success = await _bookingService.CancelAsync(id);
            if (!success) return NotFound();

            // JELASKAN: 204 No Content — sesuai kontrak API
            // Frontend (BUKAN 200 OK seperti di slide asli).
            return NoContent();
        }
    }

}
