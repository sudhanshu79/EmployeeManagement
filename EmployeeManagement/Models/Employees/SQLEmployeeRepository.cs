using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models.Employees
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        public SQLEmployeeRepository(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        public AppDbContext AppDbContext { get; }

        public Employee AddEmployee(Employee employee)
        {
            AppDbContext.Employees.Add(employee);
            AppDbContext.SaveChanges();
            return employee;
        }

        public Employee DeleteEmployee(int employeeId)
        {
            var fetchedEmployee = AppDbContext.Employees.Find(employeeId);
            if(fetchedEmployee != null)
            {
                AppDbContext.Employees.Remove(fetchedEmployee);
                AppDbContext.SaveChanges();
            }
            return fetchedEmployee;

        }

        public Employee GetEmployee(int id)
        {
            return AppDbContext.Employees.Find(id);
        }

        public List<Employee> GetEmployeesList()
        {
            return AppDbContext.Employees.ToList();
        }

        public Employee UpdateEmployee(Employee employee)
        {
            var employeeEnrty = AppDbContext.Employees.Attach(employee);
            employeeEnrty.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            AppDbContext.SaveChanges();
            return employee;
        }
    }
}
