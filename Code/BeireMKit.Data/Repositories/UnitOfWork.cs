using BeireMKit.Data.Interfaces;

namespace BeireMKit.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected readonly IBaseDbContext _context;
        private bool _disposed;

        public UnitOfWork(IBaseDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }
    }
}
