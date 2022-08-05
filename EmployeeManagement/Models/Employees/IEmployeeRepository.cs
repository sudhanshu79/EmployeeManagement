using System.Collections.Generic;

namespace EmployeeManagement.Models.Employees
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployeesList();

        Employee GetEmployee(int id);

        Employee AddEmployee(Employee employee);

        Employee UpdateEmployee(Employee employee);

        Employee DeleteEmployee(int employeeId);

    }
}
