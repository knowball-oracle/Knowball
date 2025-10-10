using Knowball.Application.Services;
using Knowball.Domain.Repositories;
using Knowball.Infrastructure;
using Knowball.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");


connectionString = connectionString.Replace("${DB_USER}", dbUser ?? "");
connectionString = connectionString.Replace("${DB_PASSWORD}", dbPassword ?? "");

builder.Services.AddDbContext<KnowballContext>(options =>
    options.UseOracle(connectionString));

builder.Services.AddScoped<ICampeonatoRepository, CampeonatoRepository>();
builder.Services.AddScoped<IEquipeRepository, EquipeRepository>();
builder.Services.AddScoped<IArbitroRepository, ArbitroRepository>();
builder.Services.AddScoped<IPartidaRepository, PartidaRepository>();
builder.Services.AddScoped<IParticipacaoRepository, ParticipacaoRepository>();
builder.Services.AddScoped<IArbitragemRepository, ArbitragemRepository>();
builder.Services.AddScoped<IDenunciaRepository, DenunciaRepository>();

builder.Services.AddScoped<ICampeonatoService, CampeonatoService>();
builder.Services.AddScoped<IPartidaService, PartidaService>();
builder.Services.AddScoped<IDenunciaService, DenunciaService>();
builder.Services.AddScoped<IEquipeService, IEquipeService>();
builder.Services.AddScoped<IArbitroService, IArbitroService>();
builder.Services.AddScoped<IParticipacaoService, IParticipacaoService>();
builder.Services.AddScoped<IArbitragemService, IArbitragemService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();