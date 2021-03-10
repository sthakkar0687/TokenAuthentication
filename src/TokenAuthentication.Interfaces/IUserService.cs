using System.Threading.Tasks;
using TokenAuthentication.Dtos;
using TokenAuthentication.Dtos.Authentication;
using TokenAuthentication.Entity.Authentication;

namespace TokenAuthentication.Common.Interface
{
    public interface IUserService
    {
        Task<ResponseDto<RegistrationResponseDto>> CreateAsync(RegistrationModel model);
        Task<ResponseDto<string>> GetUserIdByEmailAsync(string email);
    }
}
