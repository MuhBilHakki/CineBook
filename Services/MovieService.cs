

using CineBook.Models;
using CineBook.Repositories;

namespace CineBook.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;

        public MovieService(IRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<List<Movie>> GetAllAsync(string? search, int page, int limit)
        {
            var movies = await _movieRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(search))
            {
                movies = movies
                    .Where(m => m.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return movies
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
        }

        public async Task<Movie?> GetByIdAsync(long id)
        {
            return await _movieRepository.GetByIdAsync(id);
        }
    }

}
