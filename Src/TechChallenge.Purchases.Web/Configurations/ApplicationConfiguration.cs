using TechChallenge.Purchases.Application.Contracts;
using TechChallenge.Purchases.Application.Security;
using TechChallenge.Purchases.Application.Services;

namespace TechChallenge.Purchases.Web.Configurations
{
    public static class ApplicationConfiguration
    {
        public static void AddApplicationConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        }
    }
}
