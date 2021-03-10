using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokenAuthentication.Common.Constants;
using TokenAuthentication.Common.Interface;
using TokenAuthentication.Dtos;
using TokenAuthentication.Dtos.Authentication;
using TokenAuthentication.Entity.Authentication;

namespace TokenAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<UserRoleResponseDto>>> CreateAsync(UserRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto<UserRoleResponseDto>() { Message = Messages.INVALID_MODEL, Errors = ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage), StatusCode = System.Net.HttpStatusCode.BadRequest });
            return Ok(await _userRoleService.CreateAsync(model));
        }
    }
}
