using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenAuthentication.Dtos;

namespace TokenAuthentication.Interfaces
{
    public interface IDepartmentService
    {
        Task<ResponseDto<DepartmentDto>> Create(DepartmentDto DepartmentDto);
        Task<ResponseDto<DepartmentDto>> Update(Guid id, DepartmentDto DepartmentDto);
        Task<ResponseDto<DepartmentDto>> Delete(Guid id);
        Task<ResponseDto<DepartmentDto>> GetById(Guid id);
        Task<ResponseDto<ICollection<DepartmentDto>>> GetAll();
    }
}
