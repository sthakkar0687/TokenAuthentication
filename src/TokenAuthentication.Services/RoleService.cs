using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<ResponseDto<RoleResponseDto>> CreateAsync(RoleModel model)
        {
            var role = new ApplicationRole()
            {
                Name = model.RoleName,
                NormalizedName = model.RoleName.ToUpper()
            };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return new ResponseDto<RoleResponseDto>()
                {
                    Message = "Error while creating the role.",
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = result.Errors.Select(p => p.Description)
                };
            }
            return new ResponseDto<RoleResponseDto>()
            {
                Message = "Role created successfully.",
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Id = role.Id,
                Data = new RoleResponseDto()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                }
            };
        }

        public async Task<ResponseDto<string>> GetRoleIdByNameAsync(string name)
        {
            var roleId = (await _roleManager.FindByNameAsync(name))?.Id ?? null;
            return new ResponseDto<string>()
            {
                Data = Convert.ToString(roleId),
                Success = true,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public async Task<ResponseDto<IEnumerable<RoleResponseDto>>> GetAll()
        {
            var roleModelList = new List<RoleResponseDto>();
            var roles = _roleManager.Roles.ToList();
            foreach (var role in roles)
            {
                var roleModel = new RoleResponseDto() { RoleId = role.Id, RoleName = role.Name };
                roleModelList.Add(roleModel);
            }
            return new ResponseDto<IEnumerable<RoleResponseDto>>()
            {
                Data = roleModelList,
                Success = true,
                StatusCode = HttpStatusCode.OK,
            };
        }
    }
}
