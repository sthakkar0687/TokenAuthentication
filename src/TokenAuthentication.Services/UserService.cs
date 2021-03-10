using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TokenAuthentication.Common.Interface;
using TokenAuthentication.Dtos;
using TokenAuthentication.Dtos.Authentication;
using TokenAuthentication.Entity.Authentication;
using TokenAuthentication.Entity.Entity;

namespace TokenAuthentication.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<ResponseDto<RegistrationResponseDto>> CreateAsync(RegistrationModel model)
        {
            var user = new ApplicationUser()
            {
                Email = model.Email,
                DOB = model.DOB,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Errors.Count() > 0)
            {
                return new ResponseDto<RegistrationResponseDto>()
                {
                    Message = "Error while registering the user.",
                    Errors = result.Errors.Select(p => p.Description),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var role = _roleManager.Roles.FirstOrDefault(p => p.Id == model.RoleId);
            if(role!=null)
            {
                await _userManager.AddToRoleAsync(user,role.Name);
            }
            return new ResponseDto<RegistrationResponseDto>()
            {
                Data = new RegistrationResponseDto()
                {
                    UserId = user.Id,
                    Email = user.Email
                },
                Message = "User registered successfully.",
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<string>> GetUserIdByEmailAsync(string email)
        {
            var userId = (await _userManager.FindByEmailAsync(email))?.Id ?? null;
            return new ResponseDto<string>()
            {
                Data = Convert.ToString(userId),
                Success = true,
                StatusCode = HttpStatusCode.OK,
            };
        }
    }
}
