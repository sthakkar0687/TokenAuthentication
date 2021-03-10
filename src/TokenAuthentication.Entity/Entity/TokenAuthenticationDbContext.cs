using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuthentication.Entity.Entity
{
    public class TokenAuthenticationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public TokenAuthenticationDbContext(DbContextOptions<TokenAuthenticationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser()
                {
                    Id = Guid.Parse("F0F695B5-08E7-478B-8382-34AEE977C891"),
                    Email = "viralthakkar@gmail.com",
                    UserName = "viralthakkar@gmail.com",
                    FirstName = "Viral",
                    LastName = "Thakkar",
                    DOB = new DateTime(1959, 09, 24),
                    PhoneNumber = "9227218499",
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Pass@123")
                },
                new ApplicationUser()
                {
                    Id = Guid.Parse("8F9341E3-188D-4D74-A884-8F44D8BD8F64"),
                    Email = "nayanathakkar@gmail.com",
                    UserName = "nayanathakkar@gmail.com",
                    FirstName = "Nayana",
                    LastName = "Thakkar",
                    DOB = new DateTime(1961, 06, 27),
                    PhoneNumber = "9227218499",
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Pass@123")
                });
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole() { Id = Guid.Parse("7B1B1B6A-4CB5-4DFE-B72A-08D8DCB4FBEF"), Name = "Admin", NormalizedName = "ADMIN"},
                new ApplicationRole() { Id = Guid.Parse("99AB4986-9797-4E06-E62C-08D8DCB4DDEF"), Name = "User", NormalizedName = "USER" }
                );
            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>()
                {
                    RoleId = Guid.Parse("99AB4986-9797-4E06-E62C-08D8DCB4DDEF"),
                    UserId = Guid.Parse("8F9341E3-188D-4D74-A884-8F44D8BD8F64")
                },
                new IdentityUserRole<Guid>()
                {
                    RoleId = Guid.Parse("7B1B1B6A-4CB5-4DFE-B72A-08D8DCB4FBEF"),
                    UserId = Guid.Parse("8F9341E3-188D-4D74-A884-8F44D8BD8F64")
                },
                new IdentityUserRole<Guid>()
                {
                    RoleId = Guid.Parse("99AB4986-9797-4E06-E62C-08D8DCB4DDEF"),
                    UserId = Guid.Parse("F0F695B5-08E7-478B-8382-34AEE977C891")
                });
            base.OnModelCreating(builder);
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
