using CineBook.DTOs;
using CineBook.Exceptions;
using CineBook.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost] // POST /api/bookings
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            long userId = long.Parse(User.FindFirst("userId")!.Value);

           
            try
            {
                BookingDetailResponse? result = await _bookingService.CreateAsync(request, userId);
                return Created($"/api/bookings/{result!.Id}", result);
            }
            catch (SeatNotAvailableException ex)
            {
                return Conflict(new {status = "error", message = ex.Message});
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { status = "error", message = ex.Message });
            }
            
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
