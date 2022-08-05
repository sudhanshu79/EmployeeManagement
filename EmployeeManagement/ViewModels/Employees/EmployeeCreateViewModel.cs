using EmployeeManagement.Models.Employees;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels.Employees
{
    public class EmployeeCreateViewModel
    {

        [Required(ErrorMessage = "Please enter name")]
        [MaxLength(30, ErrorMessage = "Name is too long,30 characters are allowed")]
        [Display(Name = "Name")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmployeeEmail { get; set; }
        [Required(ErrorMessage = "Please select a department")]
        [Display(Name = "Department")]
        public Dept? EmployeeDept { get; set; }

        public IFormFile Photo { get; set; }
    }
}
