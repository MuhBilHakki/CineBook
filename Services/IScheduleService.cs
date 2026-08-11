using CineBook.DTOs;

namespace CineBook.Services
{
    public interface IScheduleService
    {
        Task<List<ScheduleResponse>> GetAllAsync();
        Task<ScheduleResponse?> GetByIdAsync(long id);
    }

}
