using ContratoApp.Aplicacao;
using ContratoApp.Aplicacao.Extensions;
using ContratoApp.Dominio.Events;
using ContratoApp.Funcionalidades;
using ContratoApp.Infra.Database;
using ContratoApp.Infra.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("Db");
    options.AddInterceptors(new AuditableInterceptor());
});

builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.RegisterHandlersFromAssemblyContaining(typeof(AssemblyReference));
builder.Services.AddEndpoints(typeof(AssemblyReference).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapEndpoints();

await app.RunAsync().ConfigureAwait(false);
