using CineBook.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineBook.Controllers
{
    [ApiController]
    [Route("api/schedules")]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchedules()
        {
            var schedules = await _scheduleService.GetAllAsync();
            return Ok(schedules);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetSchedule(long id)
        {
            var schedule = await _scheduleService.GetByIdAsync(id);
            if (schedule == null) return NotFound();
            return Ok(schedule);
        }
    }

}
