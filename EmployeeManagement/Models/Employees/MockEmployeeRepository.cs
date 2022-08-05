using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models.Employees
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> Employees { get; set; }

        public MockEmployeeRepository()
        {
            Employees = new List<Employee>();
            Employees.Add(new Employee() { EmployeeId = 101, EmployeeName = "Sudhanshu Sharma", EmployeeDept = Dept.Mobility, EmployeeEmail = "sudhanshu.s@ventla.io" });
            Employees.Add(new Employee() { EmployeeId = 102, EmployeeName = "Gopal Awasthi", EmployeeDept = Dept.Mobility, EmployeeEmail = "gopal@ventla.io" });
            Employees.Add(new Employee() { EmployeeId = 103, EmployeeName = "Arun Mehra", EmployeeDept = Dept.Mobility, EmployeeEmail = "arun@yopmail.com" });
            Employees.Add(new Employee() { EmployeeId = 104, EmployeeName = "Sakshi Gupta", EmployeeDept = Dept.QA, EmployeeEmail = "Sakshi@mailinaor.com" });
            Employees.Add(new Employee() { EmployeeId = 105, EmployeeName = "Neelam Singh", EmployeeDept = Dept.QA, EmployeeEmail = "neelam@gmail.com" });
            Employees.Add(new Employee() { EmployeeId = 106, EmployeeName = "Khwahish Batra", EmployeeDept = Dept.Admin, EmployeeEmail = "khwahish@ventla.io" });
        }
        public Employee GetEmployee(int id)
        {
            return Employees.FirstOrDefault(x => x.EmployeeId == id);
        }

        public List<Employee> GetEmployeesList()
        {
            return Employees;
        }

        public Employee AddEmployee(Employee employee)
        {
            employee.EmployeeId = Employees.Max(x => x.EmployeeId) + 1;
            Employees.Add(employee);
            return employee;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            var fetchedEmployee = Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            if (fetchedEmployee != null)
            {
                fetchedEmployee.EmployeeEmail = employee.EmployeeEmail;
                fetchedEmployee.EmployeeDept = employee.EmployeeDept;
                fetchedEmployee.EmployeeName = employee.EmployeeName;
            }
            return fetchedEmployee;
        }

        public Employee DeleteEmployee(int employeeId)
        {
            var fetchedEmployee = Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
            if (fetchedEmployee != null)
            {
                Employees.Remove(fetchedEmployee);
            }
            return fetchedEmployee;
        }
    }
}
