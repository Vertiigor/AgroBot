using System.ComponentModel.DataAnnotations;

namespace AgroBot.Models
{
    public enum CultureStatus
    {
        Draft,
        Active
    }

    public class Culture
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime AddedTime { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public CultureStatus Status { get; set; }
    }
}
