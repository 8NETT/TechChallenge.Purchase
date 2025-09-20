namespace TechChallenge.Purchases.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUsuarioRepository UsuarioRepository { get; }
        Task CommitAsync();
    }
}
