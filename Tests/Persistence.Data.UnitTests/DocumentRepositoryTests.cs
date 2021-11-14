using Microsoft.EntityFrameworkCore;
using Persistence.Data.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace Persistence.Data.UnitTests
{
    public class DocumentRepositoryTests : FakeDataContext
    {
        private readonly DocumentRepository _repo;

        public DocumentRepositoryTests()
             : base(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "unitTest")
                .Options)
        {
            _repo = new DocumentRepository(new DataContext(ContextOptions));
        }

        [Fact]
        public async Task GetAllAsync_WhenCalled_WillReturnCorrectDataFromDb()
        {
            var documents = await _repo.GetAllAsync();
            Assert.Equal(8, documents.Count);
        }
    }
}