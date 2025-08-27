using System.Linq.Expressions;

namespace Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        public List<T> GetAll();
        public List<T> GetAllInclude(params Expression<Func<T, object>>[] includes);
        public List<T> GetAllIncludeOrderBy(Expression<Func<T, object>> orderBy, bool ascending = true, params Expression<Func<T, object>>[] includes);
        public Task<List<T>> GetAllAsync();
        public Task<List<T>> GetAllAsyncInclude(params Expression<Func<T, object>>[] includes);
        public Task<List<T>> GetAllAsyncIncludeOrderBy(Expression<Func<T, object>> orderBy, bool ascending = true, params Expression<Func<T, object>>[] includes);
        public void Create(T entity);
        public Task<int> CreateAsync(T entity);
        void Update(T entity);
        public Task<int> UpdateAsync(T entity);
        public bool Remove(T entity);
        public Task<bool> RemoveAsync(T entity);
        public T? GetById(int? id);
        public T? GetByIdInclude<TKey>(TKey id, params Expression<Func<T, object>>[] includes);
        public Task<T?> GetByIdAsync(int? id);
        public Task<T?> GetByIdAsyncInclude<TKey>(TKey id, params Expression<Func<T, object>>[] includes);
        public void PrepareCreate(T entity);
        public void PrepareUpdate(T entity);
        public void PrepareRemove(T entity);
        public int Save();
        public Task<int> SaveAsync();
        public IQueryable<T> GetSet();
        public IEnumerable<T> Search(Expression<Func<T, bool>> predicate);
        public IEnumerable<T> SearchInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        public IEnumerable<T> SearchIncludeOrderBy(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, bool ascending = true, params Expression<Func<T, object>>[] includes);
        public Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate);
        public Task<IEnumerable<T>> SearchAsyncInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        public Task<IEnumerable<T>> SearchAsyncIncludeOrderBy(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, bool ascending = true, params Expression<Func<T, object>>[] includes);
    }
}
