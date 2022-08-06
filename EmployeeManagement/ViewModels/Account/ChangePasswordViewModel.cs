using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name ="Current Password")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new Password")]
        [Compare("NewPassword",ErrorMessage ="Password and Confirm Password mismatch")]
        public string ConfirmPassword { get; set; }
    }
}
