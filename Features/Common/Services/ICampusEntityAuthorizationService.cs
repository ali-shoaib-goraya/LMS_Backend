namespace Dynamic_RBAMS.Features.Common.Services
{
    public interface ICampusEntityAuthorizationService
    {
        Task<bool> HasAccessToEntityAsync<TEntity>(int entityId) where TEntity : class;
    }
}
