using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contracts;
using Persistence.Data.Models;

namespace Persistence.Data.Repositories
{
    public class KeywordDocumentMappingRepository : IKeywordDocumentMappingRepository
    {
        private readonly DataContext _dataContext;

        public KeywordDocumentMappingRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<List<KeywordDocumentMapping>> GetAllByKeywordIdAsync(int keywordId)
        {
            return _dataContext.KeywordDocumentMappings.Where(kwd => kwd.KeywordId == keywordId).ToListAsync();
        }

        public Task<List<Document>> GetDocumentsByKeywordIdAsync(int keywordId)
        {
            return _dataContext.KeywordDocumentMappings.Where(kwd => kwd.KeywordId == keywordId)
                .Join(
                    _dataContext.Documents,
                    keywordDocumentMapping => keywordDocumentMapping.DocumentId,
                    document => document.Id,
                    (keywordDocumentMapping, document) => new Document
                    {
                        Id = keywordDocumentMapping.DocumentId,
                        Name = document.Name,
                        Url = document.Url
                    }
                ).ToListAsync();
        }

        public Task AddRangeAsync(int keywordId, List<int> documentIds)
        {
            var newMappings = new List<KeywordDocumentMapping>();

            foreach (var documentId in documentIds)
            {
                newMappings.Add(new KeywordDocumentMapping
                {
                    KeywordId = keywordId,
                    DocumentId = documentId,
                    LastModified = DateTime.Now
                });
            }

            _dataContext.KeywordDocumentMappings.AddRange(newMappings);
            return _dataContext.SaveChangesAsync();
        }

        public Task DeleteRangeAsync(int keywordId, List<int> documentIds)
        {
            var mappings = _dataContext.KeywordDocumentMappings.Where(
                kwd => kwd.KeywordId == keywordId && documentIds.Contains(kwd.DocumentId))
                .ToList();

            _dataContext.KeywordDocumentMappings.RemoveRange(mappings);
            return _dataContext.SaveChangesAsync();
        }
    }
}
