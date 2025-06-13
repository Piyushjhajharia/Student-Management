using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagementSystem.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all students
        [Authorize]
        public IActionResult Index(string departmentName)
        {
            var students = _context.tblStudent
                                   .Include(s => s.Department)
                                   .AsQueryable();

            if (!string.IsNullOrEmpty(departmentName))
            {
                students = students.Where(s => s.Department.DepartmentName.Contains(departmentName));
            }

            ViewBag.Username = HttpContext.Session.GetString("Username");

            // 🆕 Pass department list to view
            ViewBag.Departments = _context.tblDepartment.Select(d => d.DepartmentName).ToList();

            return View(students.ToList());
        }


        // GET: Student/Create
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_context.tblDepartment.ToList(), "DepartmentId", "DepartmentName");

            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.tblStudent.Add(student);
                _context.SaveChanges();

                // Show success message via TempData
                TempData["Success"] = "Student registered successfully!";

                return RedirectToAction("Index"); // This takes you to Student/Index
            }

            // If validation fails, return the form again
            return View(student);
        }

    }
}
