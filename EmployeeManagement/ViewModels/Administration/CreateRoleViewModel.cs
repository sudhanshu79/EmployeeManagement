using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels.Administration
{
    public class CreateRoleViewModel
    {
        [Required]

        public string RoleName { get; set; }
    }
}
