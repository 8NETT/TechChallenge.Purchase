namespace TechChallenge.Purchases.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ICompraRepository CompraRepository { get; }
        Task CommitAsync();
    }
}
