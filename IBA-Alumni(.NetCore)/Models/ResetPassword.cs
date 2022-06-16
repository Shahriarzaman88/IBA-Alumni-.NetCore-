using System.ComponentModel.DataAnnotations;

namespace IBA_Alumni_.NetCore_.Models
{
    public class ResetPassword
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters !!!")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
