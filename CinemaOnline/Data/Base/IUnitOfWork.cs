
namespace CinemaOnline.Data.Base
{
    public interface IUnitOfWork
    {
      
        Task CommitAsync();
        void Dispose();
        IEntityBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntityBase, new();
        Task RollBackAsync();
    }
}