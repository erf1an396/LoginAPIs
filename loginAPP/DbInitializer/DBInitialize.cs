using loginAPP.Context;
using loginAPP.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace loginAPP.DbInitializer
{

    public interface IDBInitialize
    {
        void Initialize();
        void SeedData();
    }

    public class DBInitialize : IDBInitialize
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DBInitialize(IServiceScopeFactory serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public void Initialize()
        {
            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {
                using (AppDbContext db = serviceScope.ServiceProvider.GetService<AppDbContext>())
                {
                    db.Database.Migrate();
                }
            }
        }



        public void SeedData()
        {
            using (IServiceScope serviceScope = _serviceScopeFactory.CreateScope())
            {

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                if (!roleManager.Roles.Any())
                {
                    roleManager.CreateAsync(new ApplicationRole { Name = "admin" }).Wait();
                    roleManager.CreateAsync(new ApplicationRole { Name = "user" }).Wait();
                }


                if (!userManager.Users.Any())
                {
                    var adminUser = new ApplicationUser
                    {
                        
                        UserName = "admin",
                        Email = "admin@example.com",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(adminUser, "Admin@123").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(adminUser, "admin").Wait();
                    }


                    //using (AppDbContext db = serviceScope.ServiceProvider.GetService<AppDbContext>())
                    //{
                    //    if (!db.Roles.Any())
                    //    {
                    //        db.Roles.AddRange(
                    //            new Role { Name = "admin" },
                    //            new Role { Name = "user" }
                    //        );
                    //        db.SaveChanges();
                    //    }

                    //    if (!db.Users.Any())
                    //    {
                    //        var adminUser = new User
                    //        {
                    //            FullName = "admin",
                    //            Password = "admin123".EncodeToMd5(),
                    //            UserName = "admin"
                    //        };
                    //        db.Users.Add(adminUser);
                    //        db.SaveChanges();

                    //        var adminRole = db.Roles.FirstOrDefault(r => r.Name == "admin");
                    //        if (adminRole != null)
                    //        {
                    //            db.UserRoles.Add(new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id });
                    //            db.SaveChanges();
                    //        }
                    //    }
                    //}
                }
            }
        }
    }
}
