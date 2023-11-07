using AppointmentApp.Data;
using AppointmentApp.Models;
using AppointmentApp.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AppointmentApp.DbInitializer
{
    public class DbInitializer:IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }

            if (_db.Roles.Any(x => x.Name == Utility.Helper.Admin))
            {
                return;
            }

            _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Doctor)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Patient)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Admin Melika"
            }, "Admin@123").GetAwaiter().GetResult();

            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
            _userManager.AddToRoleAsync(user, Helper.Admin).GetAwaiter().GetResult();

        }
    }
}
