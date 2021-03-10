using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading.Tasks;
using TokenAuthentication.Common.Interface;
using TokenAuthentication.Dtos;
using TokenAuthentication.Dtos.Authentication;
using TokenAuthentication.Entity.Authentication;
using TokenAuthentication.Entity.Entity;

namespace TokenAuthentication.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRoleService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<ResponseDto<UserRoleResponseDto>> CreateAsync(UserRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
                return new ResponseDto<UserRoleResponseDto>()
                {
                    Message = $"No user found for the given userId - {model.UserId}",
                    StatusCode = HttpStatusCode.NotFound
                };
            var role = await _roleManager.FindByIdAsync(model.RoleId.ToString());
            if (role == null)
                return new ResponseDto<UserRoleResponseDto>()
                {
                    Message = $"No role found for the given roleId - {model.RoleId}",
                    StatusCode = HttpStatusCode.NotFound
                };
            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if(!result.Succeeded)
                return new ResponseDto<UserRoleResponseDto>()
                {
                    Message = $"Error while mapping the user to role.",
                    StatusCode = HttpStatusCode.BadRequest
                };

            return new ResponseDto<UserRoleResponseDto>()
            {
                Message = $"User mapped to the role successfully.",
                StatusCode = HttpStatusCode.OK,
                Success =true,
                Data = new UserRoleResponseDto()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    UserId = user.Id,
                    UserName = user.UserName
                }
            };
        }
    }
}
