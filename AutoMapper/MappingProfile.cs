using AutoMapper;
using Dynamic_RBAMS.Features.DepartmentManagement;
using Dynamic_RBAMS.Features.DepartmentManagement.Dtos;
using Dynamic_RBAMS.Features.ProgramManagement;  
using Dynamic_RBAMS.Features.ProgramManagement.Dtos; 

namespace Dynamic_RBAMS.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Department Mappings
            CreateMap<Department, DepartmentResponseDto>();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.SchoolId, opt => opt.Ignore()); // Prevent SchoolId modification

            // Program Mappings
            CreateMap<Programs, ProgramResponseDto>();
            CreateMap<CreateProgramDto, Programs>();
            CreateMap<UpdateProgramDto, Programs>()
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore()); // Prevent DepartmentId modification
        }
    }
}
