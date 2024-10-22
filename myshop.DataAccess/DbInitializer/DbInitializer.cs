using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myshop.Utilities;

namespace myshop.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
             ApplicationDbContext context )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {

            //Migration

            try
            {
                if(_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception) 
            {
                throw;
            }

            //Roles

            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.EditorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();


                //User

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Admin@MyMarket.com",
                    Email = "Admin@MyMarket.com",
                    Name = "Aministrator",
                    PhoneNumber = "1234567890",
                    Address = "Cairo",
                    City = "Cairo"
                }, "P@$$w0rd").GetAwaiter().GetResult();

                ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u=> u.Email == "Admin@MyMarket.com");
                _userManager.AddToRoleAsync(user,SD.AdminRole).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
