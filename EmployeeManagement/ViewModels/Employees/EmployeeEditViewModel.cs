namespace EmployeeManagement.ViewModels.Employees
{
    public class EmployeeEditViewModel : EmployeeCreateViewModel
    {
        public int EmployeeId { get; set; }

        public string ExistingPhotoPath { get; set; }
    }
}
