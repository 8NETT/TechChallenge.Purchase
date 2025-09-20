using TechChallenge.Purchases.Core.Entity;
using TechChallenge.Purchases.Core.Exceptions;
using TechChallenge.Purchases.Core.Extensions;
using TechChallenge.Purchases.Core.Validators;
using FluentValidation.Results;

namespace TechChallenge.Purchases.Core.Builders
{
    public sealed class UsuarioBuilder
    {
        private Usuario _usuario = new Usuario();

        public UsuarioBuilder Id(int id) => this.Tee(b => b._usuario.Id = id);
        public UsuarioBuilder DataCriacao(DateTime data) => this.Tee(b => b._usuario.DataCriacao = data);
        public UsuarioBuilder Nome(string nome) => this.Tee(b => b._usuario.Nome = nome);
        public UsuarioBuilder Email(string email) => this.Tee(b => b._usuario.Email = email);
        public UsuarioBuilder Password(string password) => this.Tee(b => b._usuario.Password = password);
        public UsuarioBuilder Profile(bool profile) => this.Tee(b => b._usuario.Profile = profile);

        public ValidationResult Validate() =>
            new UsuarioValidator().Validate(_usuario);

        public Usuario Build()
        {
            if (!Validate().IsValid)
                throw new EstadoInvalidoException("Não é possível criar um usuário em um estado inválido.");

            return _usuario;
        }
    }
}
