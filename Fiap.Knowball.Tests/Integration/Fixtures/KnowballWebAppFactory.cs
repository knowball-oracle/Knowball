using Fiap.Knowball.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fiap.Knowball.Tests.Integration.Fixtures;

public class KnowballWebAppFactory : WebApplicationFactory<Program>
{
    private static readonly InMemoryDatabaseRoot _dbRoot = new();
    private const string DbName = "KnowballIntegrationTestDb";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            services.AddDbContext<KnowballContext>(options =>
                options.UseInMemoryDatabase(DbName, _dbRoot));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<KnowballContext>();
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