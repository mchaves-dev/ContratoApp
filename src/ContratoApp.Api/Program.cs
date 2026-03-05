using ContratoApp.Aplicacao.Extensions;
using ContratoApp.Infra.Database;
using ContratoApp.Infra.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseInMemoryDatabase("Db");
	options.AddInterceptors(new AuditableInterceptor());
});

builder.Services.RegisterHandlersFromAssemblyContaining(typeof(Program));
builder.Services.AddEndpoints();

var app = builder.Build();

if (app.Environment.IsDevelopment())
	app.MapOpenApi();

app.UseHttpsRedirection();
app.MapEndpoints();

await app.RunAsync().ConfigureAwait(false);
