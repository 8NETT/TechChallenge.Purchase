namespace TechChallenge.Purchases.Application.DTOs
{
    public class UsuarioDTO
    {
        public required int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required bool Profile { get; set; }
        public required DateTime DataCriacao { get; set; }
    }
}
