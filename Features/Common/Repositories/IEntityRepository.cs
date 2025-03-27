namespace LMS.Features.Common.Repositories
{
    public interface IEntityRepository
    {
        Task<TEntity?> GetByIdAsync<TEntity>(int entityId) where TEntity : class;
    }
}
