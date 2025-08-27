using System.Linq.Expressions;
using Repositories.Basic;
using Repositories.Interface;
using Repositories.Models;

namespace Services.Services
{
    public interface IResearchProjectService
    {  
        public Task<List<Researcher>> GetAllAsyncSub();
        public List<ResearchProject> GetAll();
        public List<ResearchProject> GetAllInclude();
        public List<ResearchProject> GetAllIncludeOrderBy(Expression<Func<ResearchProject, object>> orderBy, bool ascending = true);
        public Task<List<ResearchProject>> GetAllAsync();
        public Task<List<ResearchProject>> GetAllAsyncInclude();
        public Task<List<ResearchProject>> GetAllAsyncIncludeOrderBy(Expression<Func<ResearchProject, object>> orderBy, bool ascending = true);
        public void Create(ResearchProject entity);
        public Task<int> CreateAsync(ResearchProject entity);
        public void Update(ResearchProject entity);
        public Task<int> UpdateAsync(ResearchProject entity);
        public bool Remove(ResearchProject entity);
        public Task<bool> RemoveAsync(ResearchProject entity);
        public ResearchProject GetById(int? id);
        public ResearchProject GetByIdInclude(int? id);
        public Task<ResearchProject> GetByIdAsync(int? id);
        public Task<ResearchProject> GetByIdAsyncInclude(int? id);
        public void PrepareCreate(ResearchProject entity);
        public void PrepareUpdate(ResearchProject entity);
        public void PrepareRemove(ResearchProject entity);
        public int Save();
        public Task<int> SaveAsync();
        public IEnumerable<ResearchProject> Search(Expression<Func<ResearchProject, bool>> predicate);
        public IEnumerable<ResearchProject> SearchInclude(Expression<Func<ResearchProject, bool>> predicate);
        public IEnumerable<ResearchProject> SearchIncludeOrderBy(Expression<Func<ResearchProject, bool>> predicate, Expression<Func<ResearchProject, object>> orderBy, bool ascending = true);
        public Task<IEnumerable<ResearchProject>> SearchAsync(Expression<Func<ResearchProject, bool>> predicate);
        public Task<IEnumerable<ResearchProject>> SearchAsyncInclude(Expression<Func<ResearchProject, bool>> predicate);
        public Task<IEnumerable<ResearchProject>> SearchAsyncIncludeOrderBy(Expression<Func<ResearchProject, bool>> predicate, Expression<Func<ResearchProject, object>> orderBy, bool ascending = true);
    }
    public class ResearchProjectService : IResearchProjectService
    {
        private readonly IGenericRepository<ResearchProject> _repository;
        private readonly IGenericRepository<Researcher> _subRepository;

        public ResearchProjectService(IGenericRepository<ResearchProject> repository, IGenericRepository<Researcher> subRepository)
        {
            _repository = repository;
            _subRepository = subRepository;
        }

        public ResearchProjectService()
        {
            _repository = new GenericRepository<ResearchProject>();
            _subRepository = new GenericRepository<Researcher>();
        }

        public async Task<List<Researcher>> GetAllAsyncSub() => await _subRepository.GetAllAsync();
        public List<ResearchProject> GetAll() => _repository.GetAll();
        public List<ResearchProject> GetAllInclude() => _repository.GetAllInclude(x => x.LeadResearcher);
        public List<ResearchProject> GetAllIncludeOrderBy(Expression<Func<ResearchProject, object>> orderBy, bool ascending = true) =>
            _repository.GetAllIncludeOrderBy(orderBy, ascending, x => x.LeadResearcher);
        public async Task<List<ResearchProject>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<List<ResearchProject>> GetAllAsyncInclude() => await _repository.GetAllAsyncInclude(x => x.LeadResearcher);
        public async Task<List<ResearchProject>> GetAllAsyncIncludeOrderBy(Expression<Func<ResearchProject, object>> orderBy, bool ascending = true) =>
            await _repository.GetAllAsyncIncludeOrderBy(orderBy, ascending, x => x.LeadResearcher);
        public void Create(ResearchProject entity) => _repository.Create(entity);
        public async Task<int> CreateAsync(ResearchProject entity) => await _repository.CreateAsync(entity);
        public void Update(ResearchProject entity) => _repository.Update(entity);
        public async Task<int> UpdateAsync(ResearchProject entity) => await _repository.UpdateAsync(entity);
        public bool Remove(ResearchProject entity) => _repository.Remove(entity);
        public async Task<bool> RemoveAsync(ResearchProject entity) => await _repository.RemoveAsync(entity);
        public ResearchProject GetById(int? id) => id.HasValue ? _repository.GetById(id.Value) : null;
        public ResearchProject GetByIdInclude(int? id) => id.HasValue ? _repository.GetByIdInclude<int>(id.Value, x => x.LeadResearcher) : null;
        public async Task<ResearchProject> GetByIdAsync(int? id) => id.HasValue ? await _repository.GetByIdAsync(id.Value) : null;
        public async Task<ResearchProject> GetByIdAsyncInclude(int? id) => id.HasValue ? await _repository.GetByIdAsyncInclude<int>(id.Value, x => x.LeadResearcher) : null;
        public void PrepareCreate(ResearchProject entity) => _repository.PrepareCreate(entity);
        public void PrepareUpdate(ResearchProject entity) => _repository.PrepareUpdate(entity);
        public void PrepareRemove(ResearchProject entity) => _repository.PrepareRemove(entity);
        public int Save() => _repository.Save();
        public async Task<int> SaveAsync() => await _repository.SaveAsync();
        public IEnumerable<ResearchProject> Search(Expression<Func<ResearchProject, bool>> predicate) => _repository.Search(predicate);
        public IEnumerable<ResearchProject> SearchInclude(Expression<Func<ResearchProject, bool>> predicate) => _repository.SearchInclude(predicate, x => x.LeadResearcher);
        public IEnumerable<ResearchProject> SearchIncludeOrderBy(Expression<Func<ResearchProject, bool>> predicate, Expression<Func<ResearchProject, object>> orderBy, bool ascending = true)
            => _repository.SearchIncludeOrderBy(predicate, orderBy, ascending, x => x.LeadResearcher);
        public async Task<IEnumerable<ResearchProject>> SearchAsync(Expression<Func<ResearchProject, bool>> predicate) => await _repository.SearchAsync(predicate);
        public async Task<IEnumerable<ResearchProject>> SearchAsyncInclude(Expression<Func<ResearchProject, bool>> predicate) => await _repository.SearchAsyncInclude(predicate, x => x.LeadResearcher);
        public async Task<IEnumerable<ResearchProject>> SearchAsyncIncludeOrderBy(Expression<Func<ResearchProject, bool>> predicate, Expression<Func<ResearchProject, object>> orderBy, bool ascending = true)
            => await _repository.SearchAsyncIncludeOrderBy(predicate, orderBy, ascending, x => x.LeadResearcher);
    }
}
