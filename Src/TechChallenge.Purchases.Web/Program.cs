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

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();

app.UseErrorLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapUsuarioEndpoints();
app.MapContaEndpoints();

app.Run();