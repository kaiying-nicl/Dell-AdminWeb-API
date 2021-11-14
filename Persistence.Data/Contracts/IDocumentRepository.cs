using Persistence.Data.Models;

namespace Persistence.Data.Contracts
{
    public interface IDocumentRepository
    {
        Task<List<Document>> GetAllAsync();
    }
}
