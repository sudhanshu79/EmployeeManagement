using EmployeeManagement.Models.Employees;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(new Employee
            {
                EmployeeId = 1,
                EmployeeName = "Sudhanshu",
                EmployeeDept = Dept.Mobility,
                EmployeeEmail = "sudhanshu@ventla.io"
            },
            new Employee
            {
                EmployeeId = 2,
                EmployeeName = "Gopal",
                EmployeeDept = Dept.Admin,
                EmployeeEmail = "gopal@ventla.io"
            }
            );
        }
    }
}
