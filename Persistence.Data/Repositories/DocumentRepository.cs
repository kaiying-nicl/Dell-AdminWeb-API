using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contracts;
using Persistence.Data.Models;

namespace Persistence.Data.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DataContext _dataContext;

        public DocumentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<List<Document>> GetAllAsync()
        {
            return _dataContext.Documents.ToListAsync();
        }
    }
}
