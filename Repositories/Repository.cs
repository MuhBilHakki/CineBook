using CineBook.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CineBook.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CineBookDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(CineBookDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        //public async Task<T?> GetByIdAsync(long id)
        //{
        //    return await _dbSet.FindAsync(id);
        //}

        public async Task<T?> GetByIdAsync(long id, params string[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
        }

        //public async Task<List<T>> GetAllAsync()
        //{
        //    return await _dbSet.ToListAsync();
        //}

        public async Task<List<T>> GetAllAsync(params string[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

}
