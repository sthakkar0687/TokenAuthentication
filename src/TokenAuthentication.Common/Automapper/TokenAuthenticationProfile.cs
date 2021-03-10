using AutoMapper;
using TokenAuthentication.Dtos;
using TokenAuthentication.Entity.Entity;

namespace TokenAuthentication.Common.Automapper
{
    public class TokenAuthenticationProfile : Profile
    {
        public TokenAuthenticationProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(des=>des.Departmentname, opt=>opt.MapFrom(src=>src.Department.DepartmentName))
                .ReverseMap()
                .ForMember(des=>des.Department, src=>src.Ignore());
            CreateMap<Department, DepartmentDto>().ReverseMap();
        } 
    }
}
