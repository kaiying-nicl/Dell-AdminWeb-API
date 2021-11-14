using System.ComponentModel.DataAnnotations;

namespace Persistence.Data.Models
{
    public class Keyword
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public DateTime LastModified { get; set; }
    }
}
