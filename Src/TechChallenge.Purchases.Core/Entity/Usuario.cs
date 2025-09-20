using TechChallenge.Purchases.Core.Builders;

namespace TechChallenge.Purchases.Core.Entity;

public class Usuario : EntityBase
{
    public string Nome { get; protected internal set; }
    public string Password { get; protected internal set; }
    public string Email { get; protected internal set; }
    public bool Profile { get; protected internal set; }
    protected internal Usuario() { }

    public static UsuarioBuilder New() => new UsuarioBuilder();
}