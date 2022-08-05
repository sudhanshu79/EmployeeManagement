using EmployeeManagement.ViewModels.Employees;
using EmployeeManagement.Models.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private IEmployeeRepository EmployeeRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public EmployeesController(IEmployeeRepository employeeRepository,
                                    IWebHostEnvironment hostingEnvironment)
        {
            EmployeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        public ViewResult Index()
        {
            return View(EmployeeRepository.GetEmployeesList());
        }

        public ViewResult Details(int? id)
        {
            var employeeModel = EmployeeRepository.GetEmployee(id ?? 101);

            if (employeeModel == null)
            {
                return View("../ResourceNotFound", id);
            }
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
            employeeViewModel.Employee = employeeModel;
            return View(employeeViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult EditEmployee(int id)
        {
            var existingEmployee = EmployeeRepository.GetEmployee(id);
            if (existingEmployee == null)
            {
                return View("../ResourceNotFound", id);
            }
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                EmployeeId = id,
                EmployeeName = existingEmployee.EmployeeName,
                EmployeeDept = existingEmployee.EmployeeDept,
                EmployeeEmail = existingEmployee.EmployeeEmail,
                ExistingPhotoPath = existingEmployee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult EditEmployee(EmployeeEditViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingEmployeeData = EmployeeRepository.GetEmployee(employeeViewModel.EmployeeId);
                existingEmployeeData.EmployeeName = employeeViewModel.EmployeeName;
                existingEmployeeData.EmployeeDept = employeeViewModel.EmployeeDept;
                existingEmployeeData.EmployeeEmail = employeeViewModel.EmployeeEmail;
                if (employeeViewModel.Photo != null)
                {
                    if (!string.IsNullOrEmpty(employeeViewModel.ExistingPhotoPath))
                    {
                        var filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", employeeViewModel.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    existingEmployeeData.PhotoPath = ProcessEmployeePhoto(employeeViewModel);
                }
                var updatedEmployee = EmployeeRepository.UpdateEmployee(existingEmployeeData);
                return RedirectToAction("details", new { id = updatedEmployee.EmployeeId });
            }
            return View();
        }

        private string ProcessEmployeePhoto(EmployeeCreateViewModel employeeViewModel)
        {
            string uniqueFileName;
            var absolutePath = Path.Combine(hostingEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid() + "_" + employeeViewModel.Photo.FileName;
            using (var fileStream = new FileStream(Path.Combine(absolutePath, uniqueFileName), FileMode.Create))
            {
                employeeViewModel.Photo.CopyTo(fileStream);

            }
            return uniqueFileName;
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (employeeViewModel.Photo != null)
                {
                    uniqueFileName = ProcessEmployeePhoto(employeeViewModel);
                }
                Employee employee = new Employee()
                {
                    EmployeeName = employeeViewModel.EmployeeName,
                    EmployeeDept = employeeViewModel.EmployeeDept,
                    EmployeeEmail = employeeViewModel.EmployeeEmail,
                    PhotoPath = uniqueFileName
                };
                var addedEmployee = EmployeeRepository.AddEmployee(employee);
                return RedirectToAction("details", new { id = employee.EmployeeId });
            }
            return View();
        }
    }
}
