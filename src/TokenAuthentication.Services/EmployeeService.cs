using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TokenAuthentication.Common.Constants;
using TokenAuthentication.Dtos;
using TokenAuthentication.Entity.Entity;
using TokenAuthentication.Interfaces;

namespace TokenAuthentication.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork<TokenAuthenticationDbContext> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Employee> _employeeRepository;
        public EmployeeService(IUnitOfWork<TokenAuthenticationDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _employeeRepository = _unitOfWork.GetRepository<Employee>();
        }
        public async Task<ResponseDto<EmployeeDto>> Create(EmployeeDto employeeDto)
        {
            if (employeeDto == null)
                return new ResponseDto<EmployeeDto>() { Message = Messages.INVALID_MODEL, StatusCode = HttpStatusCode.BadRequest };
            var employee = _mapper.Map<Employee>(employeeDto);
            if (employee.DepartmentId == Guid.Empty)
                employee.DepartmentId = null;
            await _employeeRepository.AddAsync(employee);
            var result = await _unitOfWork.SaveAndCommitAsync();
            var message = (result == 0) ? Messages.EMPLOYEE_CREATE_FAILURE : Messages.EMPLOYEE_CREATE_SUCCESS;
            var success = (result == 0) ? false : true;            
            return new ResponseDto<EmployeeDto>() { Message = message, Success = success, StatusCode = HttpStatusCode.OK };
        }

        public async Task<ResponseDto<EmployeeDto>> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return new ResponseDto<EmployeeDto>() { Message = Messages.INVALID_DATA, StatusCode = HttpStatusCode.BadRequest };
            await _employeeRepository.RemoveAsync(id);
            var result = await _unitOfWork.SaveAndCommitAsync();
            var message = (result == 0) ? Messages.EMPLOYEE_DELETE_FAILURE : Messages.EMPLOYEE_DELETE_SUCCESS;
            var success = result != 0;
            return new ResponseDto<EmployeeDto>() { Message = message, Success = success, StatusCode = HttpStatusCode.OK };
        }

        public async Task<ResponseDto<ICollection<EmployeeDto>>> GetAll()
        {
            return new ResponseDto<ICollection<EmployeeDto>>()
            {
                Data = _mapper.Map<ICollection<EmployeeDto>>(await _employeeRepository.GetListAsync(null,
                                //predicate:p=>p.Age > 15,
                                orderBy: p => p.OrderBy(a => a.EmployeeName),
                                include: p => p.Include(a => a.Department))),
                Success = true,                
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<EmployeeDto>> GetById(Guid id)
        {
            return new ResponseDto<EmployeeDto>()
            {
                Data = _mapper.Map<EmployeeDto>(await _employeeRepository.FirstOrDefaultAsync(p => p.EmployeeId == id)),
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<EmployeeDto>> Update(Guid id, EmployeeDto employeeDto)
        {
            if (employeeDto == null)
                return new ResponseDto<EmployeeDto>() { Message = Messages.INVALID_MODEL, StatusCode = HttpStatusCode.BadRequest };
            if (id != employeeDto.EmployeeId || id == Guid.Empty)
                return new ResponseDto<EmployeeDto>() { Message = Messages.INVALID_MODEL, StatusCode = HttpStatusCode.BadRequest };
            var employee = _mapper.Map<Employee>(employeeDto);
            if (employee.DepartmentId == Guid.Empty)
                employee.DepartmentId = null;
            _employeeRepository.Update(employee);
            var result = await _unitOfWork.SaveAndCommitAsync();
            var message = (result == 0) ? Messages.EMPLOYEE_UPDATE_FAILURE : Messages.EMPLOYEE_UPDATE_SUCCESS;
            var success = result != 0;
            return new ResponseDto<EmployeeDto>() { Message = message, Success = success, StatusCode = HttpStatusCode.OK };
        }
    }
}
