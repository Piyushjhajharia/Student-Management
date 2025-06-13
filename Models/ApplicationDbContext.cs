using Microsoft.EntityFrameworkCore;
//using StudentManagementSystem.Models;

namespace StudentManagementSystem.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Student> tblStudent { get; set; }
        public DbSet<Department> tblDepartment { get; set; }
    }
}
