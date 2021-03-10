using Microsoft.AspNetCore.Authorization;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<RegistrationResponseDto>>> CreateAsync(RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto<RegistrationResponseDto>() { Message = Messages.INVALID_MODEL, Errors = ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage), StatusCode = System.Net.HttpStatusCode.BadRequest });
            return Ok(await _userService.CreateAsync(model));
        }

        [HttpGet("GetUserIdByEmailAsync/{email}")]
        public async Task<ActionResult<ResponseDto<string>>> GetUserIdByEmailAsync(string email)
        {            
            return Ok(await _userService.GetUserIdByEmailAsync(email));
        }
    }
}
