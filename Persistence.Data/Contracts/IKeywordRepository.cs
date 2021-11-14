using Persistence.Data.Models;

namespace Persistence.Data.Contracts
{
    public interface IKeywordRepository
    {
        Task AddAsync(Keyword keyword);
        Task UpdateAsync(Keyword keyword);
        Task DeleteAsync(int id);
        Task<List<Keyword>> GetAllAsync();
    }
}
