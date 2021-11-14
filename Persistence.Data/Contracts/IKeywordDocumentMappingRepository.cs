using Persistence.Data.Models;

namespace Persistence.Data.Contracts
{
    public interface IKeywordDocumentMappingRepository
    {
        Task<List<KeywordDocumentMapping>> GetAllByKeywordIdAsync(int keywordId);
        Task<List<Document>> GetDocumentsByKeywordIdAsync(int keywordId);
        Task AddRangeAsync(int keywordId, List<int> documentIds);
        Task DeleteRangeAsync(int keywordId, List<int> documentIds);
    }
}
