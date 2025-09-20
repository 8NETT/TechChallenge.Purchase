using TechChallenge.Purchases.Core.Repository;

namespace TechChallenge.Purchases.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        private IUsuarioRepository _usuarioRepository = null!;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUsuarioRepository UsuarioRepository =>
            _usuarioRepository = _usuarioRepository ?? new UsuarioRepository(_context);

        public async Task CommitAsync() =>
            await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
