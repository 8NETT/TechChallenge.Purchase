using TechChallenge.Purchases.Core.Entity;
using TechChallenge.Purchases.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace TechChallenge.Purchases.Infrastructure.Repository;

public class UsuarioRepository(ApplicationDbContext context) : EFRepository<Usuario>(context), IUsuarioRepository
{
    public async Task<Usuario?> ObterPorEmailAsync(string email) =>
        await _dbSet.SingleOrDefaultAsync(u => u.Email == email);

    public async Task<Usuario?> ObterComBibliotecaAsync(int id) =>
        await _dbSet.SingleOrDefaultAsync(u => u.Id == id);

    public async Task<Usuario?> ObterComComprasAsync(int id) =>
        await _dbSet.SingleOrDefaultAsync(u => u.Id == id);
}