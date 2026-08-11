using CineBook.DTOs;
using CineBook.Models;
using CineBook.Repositories;

namespace CineBook.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IRepository<Schedule> _scheduleRepository;

        public ScheduleService(IRepository<Schedule> scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task<List<ScheduleResponse>> GetAllAsync()
        {
            List<Schedule> schedules = await _scheduleRepository.GetAllAsync("Movie", "Theater");
            return schedules.Select(ToResponse).ToList();
        }

        public async Task<ScheduleResponse?> GetByIdAsync(long id)
        {
            Schedule? schedule = await _scheduleRepository.GetByIdAsync(id, "Movie", "Theater");
            return schedule != null ? ToResponse(schedule) : null;
        }

        private static ScheduleResponse ToResponse(Schedule s)
        {
            return new ScheduleResponse
            {
                Id = s.Id,
                MovieId = s.MovieId,
                MovieTitle = s.Movie?.Title ?? string.Empty,
                MovieDescription = s.Movie?.Description,
                MovieDuration = s.Movie?.Duration ?? 0,
                TheaterId = s.TheaterId,
                TheaterName = s.Theater?.Name ?? string.Empty,
                TheaterLocation = s.Theater?.Location ?? string.Empty,
                StartTime = s.StartTime,
                Price = s.Price
            };
        }
    }

}
