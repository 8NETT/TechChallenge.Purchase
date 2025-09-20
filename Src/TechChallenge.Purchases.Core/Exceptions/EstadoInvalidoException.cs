namespace TechChallenge.Purchases.Core.Exceptions
{
    public class EstadoInvalidoException : DomainException
    {
        public EstadoInvalidoException(string message) : base(message) { }
    }
}
