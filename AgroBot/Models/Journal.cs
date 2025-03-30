using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AgroBot.Models
{
    public enum JournalStatus
    {
        Draft,
        Active
    }
    public class Journal 
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string CropId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public byte[] Photo { get; set; }

        [Required]
        public int Height { get; set; }

        [Required]
        public string ObservationText { get; set; }

        [Required]
        public JournalStatus Status { get; set; }

    }

}
