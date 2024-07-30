using CinemaOnline.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore.Storage;

namespace CinemaOnline.Data.Base
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly CinemaDBContext _context;
        private Dictionary<Type, object>? _repos;
        private IDbContextTransaction? _transaction;
        private bool disposedValue;
        private bool isRollbacked = false;
        private bool isCommitted = false;

        public UnitOfWork(CinemaDBContext context)
        {
            _context = context;
            _transaction =  _context.Database.BeginTransaction();
        }


        public IEntityBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntityBase, new()
        {
            if (_repos == null)
            {
                _repos = new Dictionary<Type, object>();
            }
            var type = typeof(TEntity);
            if (!_repos.ContainsKey(type))
            {
                _repos[type] = new EntityBaseRepository<TEntity>(_context);
            }
            return (IEntityBaseRepository<TEntity>)_repos[type];
        }

 

        public async Task CommitAsync()
        {
            if (isCommitted || isRollbacked)
            {
                throw new Exception("commit or rollback has been called.");
            }
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
            isCommitted = true;

        }

        public async Task RollBackAsync()
        {
            if (isCommitted || isRollbacked)
            {
                throw new Exception("commit or rollback has been called.");
            }
            await _transaction.RollbackAsync();
            DisposeTransaction();
        }

        private void DisposeTransaction()
        {
            _transaction?.Dispose();
            _transaction = null!;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeTransaction();
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }
        ~UnitOfWork()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
