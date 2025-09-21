using TechChallenge.Purchases.Core.Repository;

namespace TechChallenge.Purchases.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        private ICompraRepository _compraRepository = null!;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICompraRepository CompraRepository =>
            _compraRepository = _compraRepository ?? new CompraRepository(_context);

        public async Task CommitAsync() =>
            await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
