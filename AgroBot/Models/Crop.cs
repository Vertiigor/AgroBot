using System.ComponentModel.DataAnnotations;

namespace AgroBot.Models
{
    public class Crop
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

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
        public DateTime AddedTime { get; set; } // added into the DB

        [Required]
        public string Substrate { get; set; }

        [Required]
        public string AdditionalInfo { get; set; } 

        [Required]
        public CropStatus Status { get; set; }
    }

    public enum CropStatus
    {
        Draft,
        Active
    }
}
