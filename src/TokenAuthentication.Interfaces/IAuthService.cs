using System.Threading.Tasks;
using TokenAuthentication.Common;
using TokenAuthentication.Dtos;
using TokenAuthentication.Dtos.Authentication;
using TokenAuthentication.Entity.Authentication;

namespace TokenAuthentication.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginModel model);        
        LoggedInUser LoggedInUser();        
    }
}
