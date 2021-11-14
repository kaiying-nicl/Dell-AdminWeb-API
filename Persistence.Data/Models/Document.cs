using System.ComponentModel.DataAnnotations;

namespace Persistence.Data.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
