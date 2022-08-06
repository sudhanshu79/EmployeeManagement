using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
