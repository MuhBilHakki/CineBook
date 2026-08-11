using CineBook.Models;
using CineBook.Repositories;

namespace CineBook.Services
{
    public class SeatService : ISeatService
    {
        private readonly IRepository<Seat> _seatRepository;

        public SeatService(IRepository<Seat> seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<List<Seat>> GetByScheduleIdAsync(long scheduleId)
        {
            var seats = await _seatRepository.GetAllAsync();
            return seats.Where(s => s.ScheduleId == scheduleId).ToList();
        }
    }

}
