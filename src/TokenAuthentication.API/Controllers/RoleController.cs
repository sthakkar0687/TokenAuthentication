using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<RoleResponseDto>>> CreateAsync(RoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto<RoleResponseDto>() { Message = Messages.INVALID_MODEL , Errors = ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage), StatusCode = System.Net.HttpStatusCode.BadRequest });
            return Ok(await _roleService.CreateAsync(model));
        }

        [HttpGet("GetRoleIdByNameAsync/{name}")]
        public async Task<ActionResult<ResponseDto<string>>> GetRoleIdByNameAsync(string name)
        {
            return Ok(await _roleService.GetRoleIdByNameAsync(name));
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<IEnumerable<RoleResponseDto>>>> GetAll()
        {
            return Ok(await _roleService.GetAll());
        }
    }
}
