using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenAuthentication.Common.Constants;
using TokenAuthentication.Dtos;
using TokenAuthentication.Interfaces;

namespace TokenAuthentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<ICollection<DepartmentDto>>>> GetAll()
        {
            return await _departmentService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<DepartmentDto>>> GetById(Guid id)
        {
            return await _departmentService.GetById(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<DepartmentDto>>> Delete(Guid id)
        {
            return await _departmentService.Delete(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<DepartmentDto>>> Update(Guid id, DepartmentDto DepartmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto<DepartmentDto>() { Message = Messages.INVALID_MODEL, Errors = ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage), StatusCode = System.Net.HttpStatusCode.BadRequest });
            return Ok(await _departmentService.Update(id, DepartmentDto));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<DepartmentDto>>> Create(DepartmentDto DepartmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto<DepartmentDto>() { Message = Messages.INVALID_MODEL, Errors = ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage), StatusCode = System.Net.HttpStatusCode.BadRequest });
            return await _departmentService.Create(DepartmentDto);
        }
    }
}
