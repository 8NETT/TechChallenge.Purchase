using TechChallenge.Purchases.Core.Entity;
using TechChallenge.Purchases.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace TechChallenge.Purchases.Infrastructure.Repository
{
    public class CompraRepository : EFRepository<Compra>, ICompraRepository
    {
        public CompraRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Compra?> ObterComCompradorBiblioteca(int id) =>
            await _dbSet
                .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<Compra>> ObterDoComprador(int usuarioId) =>
            await _dbSet.Where(c => c.CompradorId == usuarioId).ToArrayAsync();
    }
}