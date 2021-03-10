using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TokenAuthentication.Common;
using TokenAuthentication.Dtos;
using TokenAuthentication.Entity.Entity;
using TokenAuthentication.Interfaces;

namespace TokenAuthentication.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly TokenAuthenticationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly LoggedInUser _loggedInUser;
        public DepartmentService(TokenAuthenticationDbContext dbContext, IMapper mapper, IAuthService authService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _authService = authService;
            _loggedInUser = _authService.LoggedInUser();
        }
        public async Task<ResponseDto<DepartmentDto>> Create(DepartmentDto DepartmentDto)
        {
            Department Department = _mapper.Map<Department>(DepartmentDto);
            _dbContext.Departments.Add(Department);
            await _dbContext.SaveChangesAsync();
            DepartmentDto.DepartmentId = Department.DepartmentId;
            return new ResponseDto<DepartmentDto>()
            {
                Id = DepartmentDto.DepartmentId,
                Data = DepartmentDto,
                Message = "Department created successfully.",
                StatusCode = HttpStatusCode.OK,                
            };
        }

        public async Task<ResponseDto<DepartmentDto>> Delete(Guid id)
        {
            Department Department = await _dbContext.Departments.FirstOrDefaultAsync(p => p.DepartmentId == id);
            if (Department == null)
                return new ResponseDto<DepartmentDto>()
                {
                    Message = $"No department found for the given id {id}",
                    StatusCode = HttpStatusCode.NotFound,                   
                };
            _dbContext.Departments.Remove(Department);
            await _dbContext.SaveChangesAsync();
            DepartmentDto DepartmentDto = _mapper.Map<DepartmentDto>(Department);
            return new ResponseDto<DepartmentDto>()
            {
                Id = id,
                Data = DepartmentDto,
                Success = true,
                Message = "Department deleted successfully.",
                StatusCode = HttpStatusCode.OK,                
            };
        }

        public async Task<ResponseDto<ICollection<DepartmentDto>>> GetAll()
        {
            var DepartmentList = await _dbContext.Departments.ToListAsync();
            if (DepartmentList == null || DepartmentList.Count == 0)
                return new ResponseDto<ICollection<DepartmentDto>>()
                {
                    Message = "No departments found.",
                    StatusCode = HttpStatusCode.NotFound,                    
                };
            var DepartmentDtoList = _mapper.Map<ICollection<DepartmentDto>>(DepartmentList);
            return new ResponseDto<ICollection<DepartmentDto>>()
            {
                Data = DepartmentDtoList,
                Success = true,
                StatusCode = HttpStatusCode.OK,                
            };
        }

        public async Task<ResponseDto<DepartmentDto>> GetById(Guid id)
        {
            Department Department = await _dbContext.Departments.FirstOrDefaultAsync(p => p.DepartmentId == id);
            if (Department == null)
                return new ResponseDto<DepartmentDto>()
                {
                    Message = $"No department found for the given id {id}",
                    StatusCode = HttpStatusCode.NotFound,                    
                };
            DepartmentDto DepartmentDto = _mapper.Map<DepartmentDto>(Department);
            return new ResponseDto<DepartmentDto>()
            {
                Id = id,
                Data = DepartmentDto,
                Success = true,
                StatusCode = HttpStatusCode.OK,                
            };
        }

        public async Task<ResponseDto<DepartmentDto>> Update(Guid id, DepartmentDto DepartmentDto)
        {
            if (id != DepartmentDto.DepartmentId)
                return new ResponseDto<DepartmentDto>()
                {
                    Message = "Incorrect input data.",
                    StatusCode = HttpStatusCode.BadRequest,                    
                };
            Department Department = _mapper.Map<Department>(DepartmentDto);
            _dbContext.Entry(Department).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return new ResponseDto<DepartmentDto>()
            {
                Id = DepartmentDto.DepartmentId,
                Data = DepartmentDto,
                Success = true,
                Message = "Department details updated successfully.",
                StatusCode = HttpStatusCode.OK,                
            };
        }
    }
}
