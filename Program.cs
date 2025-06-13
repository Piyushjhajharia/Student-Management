using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            // Add authentication services
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            // ✅ Use authentication & authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Auth}/{id?}");

            // Seed Departments
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.tblDepartment.Any())
                {
                    context.tblDepartment.AddRange(
                        new Department { DepartmentName = "Computer Engineering" },
                        new Department { DepartmentName = "Mechanical Engineering" },
                        new Department { DepartmentName = "Electrical Engineering" },
                        new Department { DepartmentName = "Civil Engineering" },
                        new Department { DepartmentName = "Electronics & Communication" },
                        new Department { DepartmentName = "Chemical Engineering" }
                    );
                    context.SaveChanges();
                }
            }

            app.Run();
        }
    }
}
