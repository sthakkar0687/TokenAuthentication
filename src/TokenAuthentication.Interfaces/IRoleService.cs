using System.Collections.Generic;
using System.Threading.Tasks;
using TokenAuthentication.Dtos;
using TokenAuthentication.Dtos.Authentication;
using TokenAuthentication.Entity.Authentication;

namespace TokenAuthentication.Common.Interface
{
    public interface IRoleService
    {
        Task<ResponseDto<RoleResponseDto>> CreateAsync(RoleModel model);
        Task<ResponseDto<string>> GetRoleIdByNameAsync(string name);
        Task<ResponseDto<IEnumerable<RoleResponseDto>>> GetAll();
    }
}
