using TechChallenge.Purchases.Core.Entity;
using TechChallenge.Purchases.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace TechChallenge.Purchases.Infrastructure.Repository;

public class EFRepository<T> : IRepository<T> where T : EntityBase   
{
    protected ApplicationDbContext _context;
    protected DbSet<T> _dbSet;

    public EFRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> ObterTodosAsync() =>
        await _dbSet.ToArrayAsync();

    public async Task<T?> ObterPorIdAsync(int id) =>
        await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

    public int Cadastrar(T entidade)
    {
        var incluido = _dbSet.Add(entidade);
        
        return incluido.Entity.Id;
    }

    public void Alterar(T entidade) =>
        _dbSet.Update(entidade);

    public void Deletar(T entidade) =>
        _dbSet.Remove(entidade);
}