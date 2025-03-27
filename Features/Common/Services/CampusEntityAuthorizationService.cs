using LMS.Features.Common.Repositories;
using LMS.Features.Common.Services;
using LMS.Features.CourseManagement;
using LMS.Features.DepartmentManagement;
using LMS.Features.ProgramManagement;
using LMS.Features.SchoolManagement;  // Added for School entity
using LMS.Features.CampusManagement;
using System;
using LMS.Features.BatchManagement;
using LMS.Features.SectionManagement;

public class CampusEntityAuthorizationService : ICampusEntityAuthorizationService
{
    private readonly IUserContextService _userContextService;
    private readonly IEntityRepository _entityRepository;

    public CampusEntityAuthorizationService(IUserContextService userContextService, IEntityRepository entityRepository)
    {
        _userContextService = userContextService;
        _entityRepository = entityRepository;
    }

    public async Task<bool> HasAccessToEntityAsync<TEntity>(int entityId) where TEntity : class
    {
        int? campusAdminCampusId = _userContextService.GetCampusId();
        if (campusAdminCampusId == null) return false; // CampusAdmin only

        int? entityCampusId = await GetCampusIdForEntity<TEntity>(entityId);
        if (entityCampusId == null) return false; // Entity not found

        return entityCampusId == campusAdminCampusId;
    }

    private async Task<int?> GetCampusIdForEntity<TEntity>(int entityId) where TEntity : class
    {
        var entity = await _entityRepository.GetByIdAsync<TEntity>(entityId);
        if (entity == null) return null; // Entity not found

        return entity switch
        {
            Department department => department.School?.CampusId,  // Null-safe
            Programs program => program.Department?.School?.CampusId,
            ProgramBatch programBatch => programBatch.Program?.Department?.School?.CampusId,
            ProgramBatchSection programBatchSection => programBatchSection.ProgramBatch?.Program?.Department?.School?.CampusId,
            Course course => course.Department?.School?.CampusId,
            School school => school.CampusId,
            Campus campus => campus.CampusId,
            _ => throw new InvalidOperationException($"Entity type {typeof(TEntity).Name} is not supported.")
        };
    }
}
