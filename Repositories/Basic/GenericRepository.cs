using Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Repositories.Models;

namespace Repositories.Basic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected Context _context;

        public GenericRepository()
        {
            _context ??= new Context();
        }

        public GenericRepository(Context context)
        {
            _context = context;
        }

        public List<T> GetAll()
        {
            // Use fresh context to avoid caching issues
            using (var freshContext = new Context())
            {
                return freshContext.Set<T>().ToList();
            }
        }

        public List<T> GetAllInclude(params Expression<Func<T, object>>[] includes)
        {
            // Use fresh context to avoid caching issues
            using (var freshContext = new Context())
            {
                IQueryable<T> query = freshContext.Set<T>();
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return query.ToList();
            }
        }

        public List<T> GetAllIncludeOrderBy(
            Expression<Func<T, object>> orderBy,
            bool ascending = true,
            params Expression<Func<T, object>>[] includes
            )
        {
            // Use fresh context to avoid caching issues
            using (var freshContext = new Context())
            {
                IQueryable<T> query = freshContext.Set<T>();
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                query = ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
                return query.ToList();
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllAsyncInclude(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAllAsyncIncludeOrderBy(
            Expression<Func<T, object>> orderBy,
            bool ascending = true,
            params Expression<Func<T, object>>[] includes
            )
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            query = ascending
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);
            return await query.ToListAsync();
        }

        public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            // Use a fresh context for the update to avoid tracking issues
            using (var freshContext = new Context())
            {
                freshContext.ChangeTracker.Clear();
                var tracker = freshContext.Attach(entity);
                tracker.State = EntityState.Modified;
                freshContext.SaveChanges();
            }
        }

        public async Task<int> UpdateAsync(T entity)
        {
            // Use a fresh context for the update to avoid tracking issues
            using (var freshContext = new Context())
            {
                freshContext.ChangeTracker.Clear();
                var tracker = freshContext.Attach(entity);
                tracker.State = EntityState.Modified;
                return await freshContext.SaveChangesAsync();
            }
        }

        public bool Remove(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public T? GetById(int? id)
        {
            return _context.Set<T>().Find(id);
        }

        public T? GetByIdInclude<TKey>(
            TKey id,
            params Expression<Func<T, object>>[] includes
            )
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Find the actual primary key property name
            var keyName = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties[0].Name;
            return query.FirstOrDefault(e => EF.Property<TKey>(e, keyName).Equals(id));
        }

        public async Task<T?> GetByIdAsync(int? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetByIdAsyncInclude<TKey>(
            TKey id,
            params Expression<Func<T, object>>[] includes
            )
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Find the actual primary key property name
            var keyName = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties[0].Name;
            return await query.FirstOrDefaultAsync(e => EF.Property<TKey>(e, keyName).Equals(id));
        }

        public void PrepareCreate(T entity)
        {
            _context.Add(entity);
        }

        public void PrepareUpdate(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        public void PrepareRemove(T entity)
        {
            _context.Remove(entity);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public IQueryable<T> GetSet()
        {
            return _context.Set<T>();
        }
        public IEnumerable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }

        public IEnumerable<T> SearchInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(predicate).ToList();
        }
        public IEnumerable<T> SearchIncludeOrderBy(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderBy,
            bool ascending = true,
            params Expression<Func<T, object>>[] includes
            )
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            query = query.Where(predicate);
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            return query.ToList();
        }

        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> SearchAsyncInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            query = query.Where(predicate);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> SearchAsyncIncludeOrderBy(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderBy,
            bool ascending = true,
            params Expression<Func<T, object>>[] includes
        )
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            query = query.Where(predicate);
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            return await query.ToListAsync();
        }
    }
}
