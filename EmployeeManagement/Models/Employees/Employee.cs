using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models.Employees
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        [MaxLength(30, ErrorMessage = "Name is too long,30 characters are allowed")]
        [Display(Name = "Name")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Please enter valid email")]
        [Display(Name = "Email")]
        public string EmployeeEmail { get; set; }
        [Required(ErrorMessage = "Please select a department")]
        [Display(Name = "Department")]
        public Dept? EmployeeDept { get; set; }

        public string PhotoPath { get; set; }

    }
}
