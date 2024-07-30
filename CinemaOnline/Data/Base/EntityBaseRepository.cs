using CinemaOnline.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CinemaOnline.Data.Base
{
    public class EntityBaseRepository<TEntity> : IEntityBaseRepository<TEntity> where TEntity : class, IEntityBase, new()
    {
        private readonly CinemaDBContext _context;
        private readonly DbSet<TEntity> _dBSet;

        public EntityBaseRepository(CinemaDBContext context)
        {
            _context = context;
            _dBSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            if (_dBSet.Contains(entity))
                throw new InvalidOperationException($"{entity} already contains in {nameof(TEntity)}  collection");
            await _dBSet.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            TEntity? entityToDelete = await _dBSet.FindAsync(id);
            if (entityToDelete == null)
                throw new NullReferenceException($"element with id: {id} not found");
            Delete(entityToDelete);

        }

        public void Delete(TEntity entityToDelete)
        {
            if (!_dBSet.Contains(entityToDelete))
                throw new InvalidOperationException($"{entityToDelete} not in {nameof(TEntity)}  collection");
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
                _dBSet.Attach(entityToDelete);
            _dBSet.Remove(entityToDelete);
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dBSet;
            if (filter != null)
                query = query.Where(filter);
            if (includeProperties != null && includeProperties.Count() > 0)
                foreach (var includeProperty in includeProperties)
                    query = query.Include(includeProperty);
            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            else
                return await query.ToListAsync();

        }

        public async Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryEntity = _dBSet;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    queryEntity = queryEntity.Include(includeProperty);
                }
            }

            var entityToFind = await queryEntity.FirstOrDefaultAsync(t => t.Id == id);
            if (entityToFind == null)
            {
                throw new NullReferenceException($"element with id: {id} not found");
            }

            return entityToFind;
        }

        public async Task UpdateAsync(TEntity entityToUpdate)
        {
            var task = Task.Run(() =>
            {
                _dBSet.Attach(entityToUpdate);
                _context.Entry(entityToUpdate).State = EntityState.Modified;
            });

            await task;
        }
    }
}
