using TechChallenge.Purchases.Core.Entity;

namespace TechChallenge.Purchases.Core.Repository;

public interface IUsuarioRepository : IRepository<Usuario>  
{
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<Usuario?> ObterComBibliotecaAsync(int id);
    Task<Usuario?> ObterComComprasAsync(int id);
}