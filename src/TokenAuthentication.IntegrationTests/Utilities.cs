using Microsoft.AspNetCore.Identity;
using System;
using TokenAuthentication.Entity.Entity;

namespace TokenAuthentication.IntegrationTests
{
    internal class Utilities
    {
        internal static void InitializeDbForTests(TokenAuthenticationDbContext context)
        {
            var department = new Department()
            {
                DepartmentName = "Cricket",
                DepartmentId = Guid.Parse("322ec244-b5a7-480c-b6de-1e52a08980a8")
            };
            context.Departments.Add(department);
            var employee = new Employee()
            {
                EmployeeId = Guid.Parse("990003f2-ef3a-4740-a0c1-05430830066f"),
                Age = 30,
                Email = "s@s.in",
                EmployeeName = "Virat Kohli",
                Gender = "Male",
                DepartmentId = Guid.Parse("322ec244-b5a7-480c-b6de-1e52a08980a8"),                
            };
            context.Employees.Add(employee);
            var userSagar = new ApplicationUser()
            {
                FirstName = "Sagar",
                LastName = "Thakkar",
                DOB = new DateTime(1989, 10, 12),
                Email = "sagarvthakkar.12@gmail.com",
                UserName = "sagarvthakkar.12@gmail.com",
                NormalizedEmail = "SAGARVTHAKKAR.12@GMAIL.COM",
                NormalizedUserName = "SAGARVTHAKKAR.12@GMAIL.COM",
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Pass@123"),
                PhoneNumber = "9227231469",
                Id = Guid.Parse("89ef6612-d307-4d22-9157-54d680742786")
            };
            var userYogesh = new ApplicationUser()
            {
                FirstName = "Yogesh",
                LastName = "Patel",
                DOB = new DateTime(1993, 11, 15),
                Email = "yogeshpatel@gmail.com",
                UserName = "yogeshpatel@gmail.com",
                NormalizedEmail = "YOGESHPATEL@GMAIL.COM",
                NormalizedUserName = "YOGESHPATEL@GMAIL.COM",
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Pass@123"),
                PhoneNumber = "9898989898",
                Id = Guid.Parse("a4e8db96-7965-41e3-9c8b-8a0ef100575a")
            };
            var userBansari = new ApplicationUser()
            {
                FirstName = "Bansari",
                LastName = "Shah",
                DOB = new DateTime(1995, 05, 25),
                Email = "bansarishah@gmail.com",
                UserName = "bansarishah@gmail.com",
                NormalizedEmail = "BANSARISHAH@GMAIL.COM",
                NormalizedUserName = "BANSARISHAH@GMAIL.COM",
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Pass@123"),
                PhoneNumber = "9797979797",
                Id = Guid.Parse("fddfc6e0-9e30-4708-9a2f-ce2ac8726736")
            };
            var userHiral = new ApplicationUser()
            {
                FirstName = "Hiral",
                LastName = "Thakkar",
                DOB = new DateTime(1990, 02, 08),
                Email = "hiralthakkar@gmail.com",
                UserName = "hiralthakkar@gmail.com",
                NormalizedEmail = "HIRALTHAKKAR@GMAIL.COM",
                NormalizedUserName = "HIRALTHAKKAR@GMAIL.COM",
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Pass@123"),
                PhoneNumber = "9974228918",
                Id = Guid.Parse("0f7cc9dd-4141-4636-a7db-8a061cc91d2a")
            };


            var adminRole = new ApplicationRole() { Id = Guid.Parse("6952c29b-eb46-4c3a-9312-bb36f55d896c"), Name = "Admin", NormalizedName = "ADMIN" };
            var userRole = new ApplicationRole() { Id = Guid.Parse("f61564bb-e42d-44fc-9939-bec9d9c12511"), Name = "User", NormalizedName = "USER" };
            var itRole = new ApplicationRole() { Id = Guid.Parse("f6b990d1-f935-4965-94d9-6966ae2dbda7"), Name = "IT", NormalizedName = "IT" };
            var hrRole = new ApplicationRole() { Id = Guid.Parse("0de0f247-7f03-4645-b4ae-a70d4707be96"), Name = "HR", NormalizedName = "HR" };


            var identityUserRoleMappings = new IdentityUserRole<Guid>[]
            {
                new IdentityUserRole<Guid>() {RoleId = adminRole.Id,UserId = userSagar.Id},
                new IdentityUserRole<Guid>() {RoleId = userRole.Id,UserId = userHiral.Id},
                new IdentityUserRole<Guid>() {RoleId = adminRole.Id, UserId = userYogesh.Id},
                new IdentityUserRole<Guid>() {RoleId = userRole.Id, UserId = userBansari.Id}
            };
            context.Users.AddRange(new ApplicationUser[] { userHiral, userSagar, userBansari, userYogesh });
            context.Roles.AddRange(new ApplicationRole[] { userRole, adminRole, itRole, hrRole });
            context.UserRoles.AddRange(identityUserRoleMappings);
            context.SaveChanges();
        }
    }
}