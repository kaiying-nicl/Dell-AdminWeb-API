using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contracts;
using Persistence.Data.Models;

namespace Persistence.Data.Repositories
{
    public class KeywordRepository : IKeywordRepository
    {
        private readonly DataContext _dataContext;

        public KeywordRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task AddAsync(Keyword keyword)
        {
            _dataContext.Keywords.Add(keyword);
            return _dataContext.SaveChangesAsync();
        }

        public Task UpdateAsync(Keyword keyword)
        {
            var record = _dataContext.Keywords.Single(k => k.Id == keyword.Id);
            record.Value = keyword.Value;
            record.LastModified = DateTime.Now;
            return _dataContext.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            _dataContext.Keywords.Remove(_dataContext.Keywords.Single(k => k.Id == id));
            return _dataContext.SaveChangesAsync();
        }

        public Task<List<Keyword>> GetAllAsync()
        {
            return _dataContext.Keywords.ToListAsync();
        }
    }
}
