using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AgroBot.Models

{
    public class Journal 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Data{ get; set; }

        [Required]
        public byte[] Photo { get; set; }

        [Required]
        public int Height { get; set; }

        [Required]
        public string ObservationText { get; set; }

    }

}
