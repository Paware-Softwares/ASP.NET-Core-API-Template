using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User : Entity
    {
        [Required]
        public string StripeId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public string? RefreshToken { get; set; }
    }
}
