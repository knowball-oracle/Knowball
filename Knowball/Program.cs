using Knowball.Application.Services;
using Knowball.Domain.Repositories;
using Knowball.Infrastructure;
using Knowball.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

if (!string.IsNullOrEmpty(dbUser))
    connectionString = connectionString.Replace("${DB_USER}", dbUser);

if (!string.IsNullOrEmpty(dbPassword))
    connectionString = connectionString.Replace("${DB_PASSWORD}", dbPassword);

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