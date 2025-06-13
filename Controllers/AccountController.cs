using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Combined Auth View (GET)
        public IActionResult Auth()
        {
            var vm = new AuthViewModel
            {
                Student = new Student(),
                Departments = _context.tblDepartment.ToList()
            };
            return View(vm); 
        }

        // Register (POST)
        [HttpPost]
        public IActionResult Register(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.tblStudent.Add(student);
                _context.SaveChanges();
                return RedirectToAction("Auth");
            }

            var vm = new AuthViewModel
            {
                Student = student,
                Departments = _context.tblDepartment.ToList()
            };
            return View("Auth", vm);
        }

        // Login (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return RedirectToAction("Auth");
        }

        // Login (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.tblStudent
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Student");
            }

            var vm = new AuthViewModel
            {
                Student = new Student(),
                Departments = _context.tblDepartment.ToList(),
                LoginError = "Invalid username or password"
            };

            return View("Auth", vm);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Auth");
        }
    }
}
