using Dynamic_RBAMS.DTOs;
using Dynamic_RBAMS.Models;
using AutoMapper;
using Dynamic_RBAMS.DTOs.Dynamic_RBAMS.DTOs;


namespace Dynamic_RBAMS.AutoMapper
{
  
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Department, DepartmentResponseDto>();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>()
            .ForMember(dest => dest.SchoolId, opt => opt.Ignore()); // Prevent SchoolId from being modified
        }
    }

}
