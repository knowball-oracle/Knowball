using Fiap.Knowball.Infrastructure;
using Fiap.Knowball.Models;
using Fiap.Knowball.Models.Repositories;
using Fiap.Knowball.Tests.Integration.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Fiap.Knowball.Tests.Integration.Fixtures;

public class KnowballWebAppFactory : WebApplicationFactory<Program>
{
    private static readonly InMemoryDatabaseRoot _dbRoot = new();
    private const string DbName = "KnowballIntegrationTestDb";

    public HttpClient CriarClientAnonimo()
        => CreateClient();

    public HttpClient CriarClientAutenticado(string role = "Admin")
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("X-Test-Auth", "true");
        client.DefaultRequestHeaders.Add("X-Test-Role", role);
        return client;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<KnowballContext>));
            if (dbContextDescriptor != null)
                services.Remove(dbContextDescriptor);

            services.AddDbContext<KnowballContext>(options =>
                options.UseInMemoryDatabase(DbName, _dbRoot));

            var mongoDescriptors = services
                .Where(d => d.ServiceType.FullName != null &&
                            d.ServiceType.FullName.Contains("Mongo"))
                .ToList();
            foreach (var descriptor in mongoDescriptors)
                services.Remove(descriptor);

            var logDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IDenunciaLogRepository));
            if (logDescriptor != null)
                services.Remove(logDescriptor);

            services.AddScoped<IDenunciaLogRepository>(_ =>
            {
                var mock = new Mock<IDenunciaLogRepository>();

                mock.Setup(r => r.RegistrarAsync(It.IsAny<DenunciaLog>()))
                    .Returns(Task.CompletedTask);

                mock.Setup(r => r.ObterPorDenunciaAsync(It.IsAny<int>()))
                    .ReturnsAsync(new List<DenunciaLog>());

                return mock.Object;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.AuthenticationScheme, _ => { });
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<KnowballContext>();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        SeedTestData(db);

        return host;
    }

    private static void SeedTestData(KnowballContext db)
    {
        db.Campeonatos.AddRange(
            new() { IdCampeonato = 1, Nome = "Copa Sub-17", Categoria = "Sub-17", Ano = 2025 },
            new() { IdCampeonato = 2, Nome = "Copa Sub-20", Categoria = "Sub-20", Ano = 2024 }
        );

        db.Arbitros.AddRange(
            new() { IdArbitro = 1, Nome = "João Silva", Status = "Ativo" },
            new() { IdArbitro = 2, Nome = "Maria Souza", Status = "Inativo" }
        );

        db.Equipes.AddRange(
            new() { IdEquipe = 1, Nome = "Flamengo", Cidade = "Rio de Janeiro", Estado = "RJ" },
            new() { IdEquipe = 2, Nome = "Corinthians", Cidade = "São Paulo", Estado = "SP" }
        );

        db.Partidas.AddRange(
            new() { IdPartida = 1, IdCampeonato = 1, DataPartida = DateTime.Now.AddDays(5), Local = "Maracanã", PlacarMandante = 0, PlacarVisitante = 0 },
            new() { IdPartida = 2, IdCampeonato = 1, DataPartida = DateTime.Now.AddDays(10), Local = "Morumbi", PlacarMandante = 0, PlacarVisitante = 0 }
        );

        db.Participacoes.AddRange(
            new() { IdPartida = 1, IdEquipe = 1, Tipo = "Mandante" },
            new() { IdPartida = 1, IdEquipe = 2, Tipo = "Visitante" }
        );

        db.Arbitragens.AddRange(
            new() { IdPartida = 1, IdArbitro = 1, Funcao = "Principal" },
            new() { IdPartida = 2, IdArbitro = 2, Funcao = "Assistente 1" }
        );

        db.Denuncias.AddRange(
            new()
            {
                IdDenuncia = 1,
                IdPartida = 1,
                IdArbitro = 1,
                Protocolo = "PROT-2025-001",
                Relato = "Erro grave.",
                Status = "Em Análise",
                DataDenuncia = DateTime.Now
            },
            new()
            {
                IdDenuncia = 2,
                IdPartida = 1,
                IdArbitro = 2,
                Protocolo = "PROT-2025-002",
                Relato = "Segundo relato.",
                Status = "Em Análise",
                DataDenuncia = DateTime.Now
            }
        );

        db.SaveChanges();
    }
}