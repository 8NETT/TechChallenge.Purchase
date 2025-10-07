using TechChallenge.Purchases.Web.Configurations;
using TechChallenge.Purchases.Web.Endpoints;
using TechChallenge.Purchases.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configurações customizadas
builder.AddLogConfiguration();
builder.AddApiConfiguration();
builder.AddInfrastructureConfiguration();
builder.AddApplicationConfiguration();
builder.AddAuthenticationConfiguration();
builder.AddAuthorizationConfiguration();

builder.AddOpenTelemetry();

builder.AddOpenTelemetryExporters();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseErrorLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapCompraEndpoints();

app.Run();