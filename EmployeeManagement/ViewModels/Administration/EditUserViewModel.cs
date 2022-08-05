using EmployeeManagement.Utilities.CustomValidators;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels.Administration
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [ValidEmailDomain(allowedDomain: "gmail.com", ErrorMessage = "Only gmail.com domain is allowed")]
        public string Email { get; set; }

        public string City { get; set; }

        public List<string> Roles { get; set; }

        public List<string> Claims { get; set; }


    }
}
