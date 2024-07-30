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

        public UnitOfWork(CinemaDBContext context)
        {
            _context = context;
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

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();

        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();

        }

        public async Task RollBackAsync()
        {
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
