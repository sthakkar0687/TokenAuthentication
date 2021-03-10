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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Authorize(Policy = "AgeRestriction")]
        public async Task<ActionResult<ResponseDto<ICollection<EmployeeDto>>>> GetAll()
        {
            return Ok(await _employeeService.GetAll());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<ResponseDto<EmployeeDto>>> GetById(Guid id)
        {
            return Ok(await _employeeService.GetById(id));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "AgeRestriction")]
        public async Task<ActionResult<ResponseDto<EmployeeDto>>> Delete(Guid id)
        {
            return Ok(await _employeeService.Delete(id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "AgeRestriction")]
        public async Task<ActionResult<ResponseDto<EmployeeDto>>> Update(Guid id, EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto<EmployeeDto>() { Message = Messages.INVALID_MODEL, Errors = ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage), StatusCode = System.Net.HttpStatusCode.BadRequest });

            return Ok(await _employeeService.Update(id, employeeDto));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [Authorize(Policy = "AgeRestriction")]
        public async Task<ActionResult<ResponseDto<EmployeeDto>>> Create(EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseDto<EmployeeDto>() { Message = Messages.INVALID_MODEL, Errors = ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage), StatusCode = System.Net.HttpStatusCode.BadRequest });

            return Ok(await _employeeService.Create(employeeDto));
        }
    }
}
