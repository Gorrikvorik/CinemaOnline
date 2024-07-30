using System.Linq.Expressions;

namespace CinemaOnline.Data.Base
{
    public interface IEntityBaseRepository<TEntity> where TEntity : class, IEntityBase, new()
    {
        Task AddAsync(TEntity entity);
        void Delete(TEntity entityToDelete);
        Task DeleteAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task UpdateAsync(TEntity entityToUpdate);
    }
}