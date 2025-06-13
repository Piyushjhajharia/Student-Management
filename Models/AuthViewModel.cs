using System.Collections.Generic;

namespace StudentManagementSystem.Models
{
    public class AuthViewModel
    {
        public Student Student { get; set; } = new Student();
        public string LoginError { get; set; } = string.Empty;
        public List<Department> Departments { get; set; } = new List<Department>();
    }
}
