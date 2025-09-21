using TechChallenge.Purchases.Core.Repository;
using TechChallenge.Purchases.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Purchases.Core.Auth;
using TechChallenge.Purchases.Core.EventHub;
using TechChallenge.Purchases.Core.Options;
using TechChallenge.Purchases.Infrastructure.Auth;
using TechChallenge.Purchases.Infrastructure.EventHub;

namespace TechChallenge.Purchases.Web.Configurations
{
    public static class InfrastructureConfiguration
    {
        public static void AddInfrastructureConfiguration(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("ConnectionString")
                ?? throw new InvalidOperationException("ConnectionString não localizada no arquivo de configuração.");

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => { options.UseSqlServer(connectionString); }, ServiceLifetime.Scoped);

            builder.Services.Configure<EventHubOptions>(builder.Configuration.GetSection("EventHub"));
            
            builder.Services.AddSingleton<IEventHubClient, EventHubClient>();
            
            builder.Services.AddScoped<IUserContext, UserContext>();
            builder.Services.AddScoped<ICompraRepository, CompraRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
