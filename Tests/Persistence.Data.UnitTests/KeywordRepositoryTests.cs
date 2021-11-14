using Microsoft.EntityFrameworkCore;
using Persistence.Data.Models;
using Persistence.Data.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Persistence.Data.UnitTests
{
    public class KeywordRepositoryTests : FakeDataContext
    {
        private readonly KeywordRepository _repo;

        public KeywordRepositoryTests()
             : base(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "unitTest")
                .Options)
        {
            _repo = new KeywordRepository(new DataContext(ContextOptions));
        }

        [Fact]
        public async Task GetAllAsync_WhenCalled_WillReturnCorrectDataFromDb()
        {
            var keywords = await _repo.GetAllAsync();
            Assert.Equal(2, keywords.Count);
        }

        [Fact]
        public async Task Addsync_WhenCalled_WillAddtoDb()
        {
            var newKeyword = new Keyword
            {
                Value = "NiceToHave",
                LastModified = DateTime.Now
            };
            await _repo.AddAsync(newKeyword);

            var keywords= await _repo.GetAllAsync();
            Assert.True(keywords.Exists(k => k.Value == newKeyword.Value));
        }

        [Fact]
        public async Task DeleteAsync_WhenCalled_WillDeleteRecordFromDb()
        {
            var keywords = await _repo.GetAllAsync();
            int keywordIdToDelete = keywords[0].Id;
            await _repo.DeleteAsync(keywordIdToDelete);

            keywords = await _repo.GetAllAsync();
            Assert.False(keywords.Exists(k => k.Id == keywordIdToDelete));
        }

        [Fact]
        public async Task UpdateAsync_WhenCalled_WillUpdateRecordInDb()
        {
            var keywords = await _repo.GetAllAsync();
            var keywordToUpdate = keywords[0];
            await _repo.UpdateAsync(new Keyword
            {
                Id = keywordToUpdate.Id,
                Value = "TestingUpdate"
            });

            keywords = await _repo.GetAllAsync();
            Assert.Equal("TestingUpdate", keywords.Find(k => k.Id == keywordToUpdate.Id)?.Value);
        }
    }
}