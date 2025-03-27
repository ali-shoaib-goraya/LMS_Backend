using AutoMapper;
using LMS.Features.DepartmentManagement;
using LMS.Features.DepartmentManagement.Dtos;
using LMS.Features.ProgramManagement;
using LMS.Features.ProgramManagement.Dtos;
using LMS.Features.BatchManagement;
using LMS.Features.BatchManagement.Dtos;
using LMS.Features.CourseManagement;
using LMS.Features.CourseManagement.Dtos;
using LMS.Features.CampusManagement.Dtos;
using LMS.Features.SectionManagement;
using LMS.Features.SectionManagement.Dtos;
using LMS.Features.SemesterManagement.Dtos;
using LMS.Features.SemesterManagement;
using LMS.Features.CourseSectionManagement;
using LMS.Features.CourseSectionManagement.Dtos;
using LMS.Features.CourseSectionManagement.Models;

namespace LMS.AutoMapper
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

            // Batch Mappings
            CreateMap<ProgramBatch, BatchResponseDto>();
            CreateMap<CreateBatchDto, ProgramBatch>();
            CreateMap<UpdateBatchDto, ProgramBatch>()
                .ForMember(dest => dest.ProgramId, opt => opt.Ignore()); // Prevent ProgramId modification

            // Course Mappings
            CreateMap<Course, CourseResponseDto>();
            CreateMap<AddCourseDto, Course>()
                .ForMember(dest => dest.ConnectedCourseId, opt => opt.Condition(src => src.ConnectedCourseId != null)); // Map only if not null
            CreateMap<UpdateCourseDto, Course>()
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore()); // Prevent DepartmentId modification

            // ProgramBatchSection Mappings
            CreateMap<ProgramBatchSection, SectionResponseDto>();
            CreateMap<CreateSectionDto, ProgramBatchSection>();
            CreateMap<UpdateSectionDto, ProgramBatchSection>()
                .ForMember(dest => dest.ProgramBatchId, opt => opt.Ignore()); // Prevent BatchId modification

            // Semester Mappings
            CreateMap<Semester, SemesterResponseDto>(); 
            CreateMap<CreateSemesterDto, Semester>();
            CreateMap<UpdateSemesterDto, Semester>()
                .ForMember(dest => dest.CampusId, opt => opt.Ignore()); // Prevent CampusId modification

            // CourseSection Mappings
            CreateMap<CourseSection, CourseSectionResponseDto>()
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School != null ? src.School.SchoolName : null))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseName : null))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseCode : null))
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty != null ? src.Faculty.FirstName + " " + src.Faculty.LastName : null))
                .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.Semester != null ? src.Semester.SemesterName : null));
            CreateMap<CreateCourseSectionDto, CourseSection>();
            CreateMap<UpdateCourseSectionDto, CourseSection>()
                .ForMember(dest => dest.SchoolId, opt => opt.Ignore()); // Prevent SchoolId modification
        }
    }
}
