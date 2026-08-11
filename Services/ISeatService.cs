using CineBook.Models;

namespace CineBook.Services
{
    public interface ISeatService
    {
        Task<List<Seat>> GetByScheduleIdAsync(long scheduleId);
    }

}
