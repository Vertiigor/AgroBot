using System.ComponentModel.DataAnnotations;

namespace AgroBot.Models
{
    public class Crop
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ChatId { get; set; }

        [Required]
        public string Culture { get; set; }

        [Required]
        public DateTime SowingDate { get; set; }

        [Required]
        public DateTime CollectionDate { get; set; }

        [Required]
        public string Substrate { get; set; }
    }
}
