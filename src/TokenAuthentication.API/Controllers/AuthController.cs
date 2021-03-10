using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokenAuthentication.Common.Constants;
using TokenAuthentication.Dtos;
using TokenAuthentication.Dtos.Authentication;
using TokenAuthentication.Entity.Authentication;
using TokenAuthentication.Interfaces;

namespace TokenAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("LoginAsync")]
        public async Task<ActionResult<ResponseDto<LoginResponseDto>>> LoginAsync(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto<LoginResponseDto> { Message = Messages.INVALID_MODEL, Errors = ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage), StatusCode = System.Net.HttpStatusCode.BadRequest });
            return Ok(await _authService.LoginAsync(model));
        }        
    }
}
