using Microsoft.EntityFrameworkCore;
using Persistence.Data.Models;

namespace Persistence.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<KeywordDocumentMapping> KeywordDocumentMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Keyword>()
                .HasIndex(k => k.Value).IsUnique();

            modelBuilder.Entity<KeywordDocumentMapping>()
                .HasKey(kdm => new { kdm.KeywordId, kdm.DocumentId });

            modelBuilder.Entity<Keyword>().HasData(
                new Keyword { Id = 1, Value = "Computer", LastModified = DateTime.Now },
                new Keyword { Id = 2, Value = "Accessories", LastModified = DateTime.Now }
            );

            modelBuilder.Entity<Document>().HasData(
                new Document { Id = 1, Name = "LaptopBrochure", Url = "https://www.google.com" },
                new Document { Id = 2, Name = "MouseBrochure", Url = "https://www.google.com" },
                new Document { Id = 3, Name = "KeyboardBrochure", Url = "https://www.google.com" },
                new Document { Id = 4, Name = "HeadphonesBrochure", Url = "https://www.google.com" },
                new Document { Id = 5, Name = "USBBrochure", Url = "https://www.google.com" },
                new Document { Id = 6, Name = "GamingMouseBrochure", Url = "https://www.google.com" },
                new Document { Id = 7, Name = "BluetoothMouseBrochure", Url = "https://www.google.com" },
                new Document { Id = 8, Name = "PCBrochure", Url = "https://www.google.com" }
            );

            modelBuilder.Entity<KeywordDocumentMapping>().HasData(
                new KeywordDocumentMapping { KeywordId = 1, DocumentId = 1, LastModified = DateTime.Now },
                new KeywordDocumentMapping { KeywordId = 1, DocumentId = 8, LastModified = DateTime.Now },
                new KeywordDocumentMapping { KeywordId = 2, DocumentId = 2, LastModified = DateTime.Now },
                new KeywordDocumentMapping { KeywordId = 2, DocumentId = 3, LastModified = DateTime.Now }
            );
        }
    }
}