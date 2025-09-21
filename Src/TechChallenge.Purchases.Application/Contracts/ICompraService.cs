using Ardalis.Result;
using TechChallenge.Purchases.Application.DTOs;
using TechChallenge.Purchases.Application.Record;

namespace TechChallenge.Purchases.Application.Contracts
{
    public interface ICompraService : IDisposable
    {
        Task<Result<int>> ComprarAsync(CompraInput input);
        Task<Result> EstornarAsync(int compraId);
    }
}