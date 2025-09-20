using Ardalis.Result;
using TechChallenge.Purchases.Application.Contracts;
using TechChallenge.Purchases.Application.DTOs;
using TechChallenge.Purchases.Application.Mappers;
using TechChallenge.Purchases.Application.Security;
using TechChallenge.Purchases.Core.Repository;

namespace TechChallenge.Purchases.Application.Services
{
    public class UsuarioService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : BaseService, IUsuarioService
    {
        public async Task<IEnumerable<UsuarioDTO>> ObterTodosAsync()
        {
            var usuarios = await unitOfWork.UsuarioRepository.ObterTodosAsync();
            return usuarios.Select(u => u.ToDTO());
        }

        public async Task<Result<UsuarioDTO>> ObterPorIdAsync(int id)
        {
            var usuario = await unitOfWork.UsuarioRepository.ObterPorIdAsync(id);

            if (usuario == null)
                return Result.NotFound("Usuário não localizado.");

            return usuario.ToDTO();
        }

        public async Task<Result<UsuarioDTO>> CadastrarAsync(CadastrarUsuarioDTO dto)
        {
            if (!TryValidate(dto, out var validationResult))
                return validationResult;

            if (await ExisteUsuarioComEmailAsync(dto.Email))
                return Result.Conflict("Já existe um usuário cadastrado com esse email.");

            var passwordHash = passwordHasher.Hash(dto.Password);
            var entidade = dto.ToEntity(passwordHash);

            unitOfWork.UsuarioRepository.Cadastrar(entidade);
            await unitOfWork.CommitAsync();

            return entidade.ToDTO();
        }

        public async Task<Result<UsuarioDTO>> AlterarAsync(AlterarUsuarioDTO dto)
        {
            if (!TryValidate(dto, out var validationResult))
                return validationResult;

            var usuario = await unitOfWork.UsuarioRepository.ObterPorIdAsync(dto.Id);

            if (usuario == null)
                return Result.NotFound("Usuário não localizado.");
            if (dto.Email != usuario.Email && await ExisteUsuarioComEmailAsync(dto.Email))
                return Result.Conflict("Já existe um usuário cadastrados com esse email.");

            var passwordHash = passwordHasher.Hash(dto.Password);
            var entidade = dto.ToEntity(usuario, passwordHash);

            unitOfWork.UsuarioRepository.Alterar(entidade);
            await unitOfWork.CommitAsync();

            return entidade.ToDTO();
        }

        public async Task<Result> DeletarAsync(int id)
        {
            var usuario = await unitOfWork.UsuarioRepository.ObterPorIdAsync(id);

            if (usuario == null)
                return Result.NotFound();

            unitOfWork.UsuarioRepository.Deletar(usuario);
            await unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result<UsuarioDTO>> LoginAsync(LoginDTO dto)
        {
            if (!TryValidate(dto, out var validationResult))
                return validationResult;

            var usuario = await unitOfWork.UsuarioRepository.ObterPorEmailAsync(dto.Email);

            if (usuario == null)
                return Result.Unauthorized();
            if (!passwordHasher.Verify(dto.Password, usuario.Password))
                return Result.Unauthorized();

            return usuario.ToDTO();
        }

        public void Dispose() => unitOfWork.Dispose();

        private async Task<bool> ExisteUsuarioComEmailAsync(string email) =>
            await unitOfWork.UsuarioRepository.ObterPorEmailAsync(email) != null;
    }
}
