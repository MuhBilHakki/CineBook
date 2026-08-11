using CineBook.Models;

namespace CineBook.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAllAsync(string? search, int page, int limit);
        Task<Movie?> GetByIdAsync(long id);
    }

}
