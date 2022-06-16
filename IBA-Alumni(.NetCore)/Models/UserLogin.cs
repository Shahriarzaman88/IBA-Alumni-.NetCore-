using System.ComponentModel.DataAnnotations;

namespace IBA_Alumni_.NetCore_.Models
{
    public class UserLogin
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
