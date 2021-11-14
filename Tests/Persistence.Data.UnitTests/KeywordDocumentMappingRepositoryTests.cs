using Microsoft.EntityFrameworkCore;
using Persistence.Data.Models;
using Persistence.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Persistence.Data.UnitTests
{
    public class KeywordDocumentMappingRepositoryTests : FakeDataContext
    {
        private readonly KeywordDocumentMappingRepository _repo;

        public KeywordDocumentMappingRepositoryTests()
            : base(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "unitTest")
                .Options)
        {
            _repo = new KeywordDocumentMappingRepository(new DataContext(ContextOptions));
        }

        [Fact]
        public async Task GetAllByKeywordIdAsync_WhenCalled_WillReturnCorrectDataFromDb()
        {
            var mappings = await _repo.GetAllByKeywordIdAsync(1);
            Assert.Equal(2, mappings.Count);
        }

        [Fact]
        public async Task GetDocumentsByKeywordIdAsync_WhenCalled_WillReturnCorrectDataFromDb()
        {
            var mappedDocuments = await _repo.GetDocumentsByKeywordIdAsync(1);
            Assert.IsType<List<Document>>(mappedDocuments);
            Assert.Equal(2, mappedDocuments.Count);
        }

        [Fact]
        public async Task AddRangeAsync_WhenCalled_WillAddtoDb()
        {
            await _repo.AddRangeAsync(1, new List<int> { 3, 4, 5 });

            var mappedDocuments = await _repo.GetDocumentsByKeywordIdAsync(1);
            Assert.Equal(5, mappedDocuments.Count);
        }

        [Fact]
        public async Task DeleteRangeAsync_WhenCalled_WillDeleteFromDb()
        {
            await _repo.DeleteRangeAsync(1, new List<int> { 1 });

            var mappedDocuments = await _repo.GetDocumentsByKeywordIdAsync(1);
            Assert.Single(mappedDocuments);
        }
    }
}
