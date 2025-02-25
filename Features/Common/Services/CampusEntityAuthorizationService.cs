using Dynamic_RBAMS.Features.Common.Repositories;
using Dynamic_RBAMS.Features.Common.Services;
using Dynamic_RBAMS.Features.CourseManagement;
using Dynamic_RBAMS.Features.DepartmentManagement;
using Dynamic_RBAMS.Features.ProgramManagement;
using Dynamic_RBAMS.Features.SchoolManagement;  // Added for School entity
using System;

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
            Course course => course.Department?.School?.CampusId,
            School school => school.CampusId,
            _ => throw new InvalidOperationException($"Entity type {typeof(TEntity).Name} is not supported.")
        };
    }
}
