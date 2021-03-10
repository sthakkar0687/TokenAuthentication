using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenAuthentication.Dtos;

namespace TokenAuthentication.Interfaces
{
    public interface IEmployeeService
    {
        Task<ResponseDto<EmployeeDto>> Create(EmployeeDto employeeDto);
        Task<ResponseDto<EmployeeDto>> Update(Guid id, EmployeeDto employeeDto);
        Task<ResponseDto<EmployeeDto>> Delete(Guid id);
        Task<ResponseDto<EmployeeDto>> GetById(Guid id);
        Task<ResponseDto<ICollection<EmployeeDto>>> GetAll();
    }
}
