namespace Persistence.Data.Models
{
    public class KeywordDocumentMapping
    {
        public int KeywordId { get; set; }

        public Keyword Keyword { get; set; }

        public int DocumentId { get; set; }

        public Document Document { get; set; }

        public DateTime LastModified { get; set; }
    }
}
