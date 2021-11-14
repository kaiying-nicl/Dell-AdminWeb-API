using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.UnitTests
{
    public class FakeDataContext
    {
        protected FakeDataContext(DbContextOptions<DataContext> contextOptions)
        {
            ContextOptions = contextOptions;
            Seed();
        }

        protected DbContextOptions<DataContext> ContextOptions { get; }

        private void Seed()
        {
            using var context = new DataContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }

}
