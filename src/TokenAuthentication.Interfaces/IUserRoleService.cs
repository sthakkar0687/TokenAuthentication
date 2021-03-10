using System.Threading.Tasks;
using TokenAuthentication.Dtos;
using TokenAuthentication.Dtos.Authentication;
using TokenAuthentication.Entity.Authentication;

namespace TokenAuthentication.Common.Interface
{
    public interface IUserRoleService
    {
        Task<ResponseDto<UserRoleResponseDto>> CreateAsync(UserRoleModel model);
    }
}
