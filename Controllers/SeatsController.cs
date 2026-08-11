using CineBook.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineBook.Controllers
{
    [ApiController]
    [Route("api/seats")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet] // GET /api/seats?scheduleId=X
        public async Task<IActionResult> GetSeats([FromQuery] long scheduleId)
        {
            var seats = await _seatService.GetByScheduleIdAsync(scheduleId);
            return Ok(seats);
        }
    }

}
