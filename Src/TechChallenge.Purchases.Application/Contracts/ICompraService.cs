using Ardalis.Result;
using TechChallenge.Purchases.Application.DTOs;

namespace TechChallenge.Purchases.Application.Contracts
{
    public interface ICompraService : IDisposable
    {
        Task<Result<int>> ComprarAsync(CompraDTO dto);
        Task<Result> EstornarAsync(int compraId);
    }
}