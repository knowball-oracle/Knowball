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
builder.Services.AddScoped<IEquipeService, EquipeService>();
builder.Services.AddScoped<IArbitroService, ArbitroService>();
builder.Services.AddScoped<IPartidaService, PartidaService>();
builder.Services.AddScoped<IParticipacaoService, ParticipacaoService>();
builder.Services.AddScoped<IArbitragemService, ArbitragemService>();
builder.Services.AddScoped<IDenunciaService, DenunciaService>();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
