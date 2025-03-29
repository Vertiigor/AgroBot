using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AgroBot.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string ChatId { get; set; }

        [Required]
        public DateTime JoinedDate { get; set; }
    }
}
