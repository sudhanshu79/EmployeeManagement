using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
