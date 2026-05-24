using System.Net;
using System.Net.Http.Json;
using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Tests.Integration.Fixtures;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Integration.Controllers;

[Collection("Integration")]
public class DenunciasApiControllerTests : IClassFixture<KnowballWebAppFactory>
{
    private readonly HttpClient _client;

    public DenunciasApiControllerTests(KnowballWebAppFactory factory)
    {
        _client = factory.CriarClientAutenticado();
    }

    [Fact]
    public async Task GetAll_SemFiltros_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/denuncias");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_IdExistente_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/denuncias/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_IdInexistente_Retorna404()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/denuncias/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_DadosValidos_Retorna201()
    {
        // Arrange
        var dto = new DenunciaDto
        {
            IdPartida = 1,
            IdArbitro = 1,
            Protocolo = "PROT-2025-099",
            Relato = "Árbitro não marcou pênalti claro.",
            Status = "Em Análise", 
            DataDenuncia = DateTime.Now
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/denuncias", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_StatusInvalido_Retorna400()
    {
        // Arrange
        var dto = new DenunciaDto
        {
            IdPartida = 1,
            IdArbitro = 1,
            Protocolo = "PROT-2025-100",
            Relato = "Relato válido.",
            Status = "Pendente",
            DataDenuncia = DateTime.Now
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/denuncias", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ProtocoloVazio_Retorna400()
    {
        // Arrange 
        var dto = new DenunciaDto
        {
            IdPartida = 1,
            IdArbitro = 1,
            Protocolo = "",
            Relato = "Relato válido.",
            Status = "Em Análise",
            DataDenuncia = DateTime.Now
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/denuncias", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_IdExistente_Retorna200()
    {
        // Arrange
        var dto = new DenunciaDto
        {
            IdDenuncia = 1,
            IdPartida = 1,
            IdArbitro = 1,
            Protocolo = "PROT-2025-001",
            Relato = "Relato atualizado com mais detalhes.",
            Status = "Em Análise",
            DataDenuncia = DateTime.Now
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/denuncias/1", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Fact]
    public async Task Update_IdInexistente_Retorna404()
    {
        var dto = new DenunciaDto
        {
            Protocolo = "PROT-001",
            Relato = "Relato.",
            Status = "Em Análise",
            DataDenuncia = DateTime.Now
        };

        var response = await _client.PutAsJsonAsync("/api/denuncias/9999", dto);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_IdExistente_Retorna200()
    {
        // Arrange
        var response = await _client.DeleteAsync("/api/denuncias/2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_IdInexistente_Retorna404()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/denuncias/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Search_ComFiltroStatus_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/denuncias/search?status=Aberta&page=1&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}