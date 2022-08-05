using EmployeeManagement.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels.Administration
{
    public class EditRoleViewodel
    {

        public string Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}
