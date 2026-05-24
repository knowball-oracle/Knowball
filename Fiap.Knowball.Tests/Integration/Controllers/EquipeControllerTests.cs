using System.Net;
using System.Net.Http.Json;
using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Tests.Integration.Fixtures;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Integration.Controllers;

[Collection("Integration")]
public class EquipeControllerTests : IClassFixture<KnowballWebAppFactory>
{
    private readonly HttpClient _client;

    public EquipeControllerTests(KnowballWebAppFactory factory)
    {
        _client = factory.CriarClientAutenticado();
    }

    [Fact]
    public async Task GetAll_SemFiltros_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/equipes");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_IdExistente_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/equipes/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_IdInexistente_Retorna404()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/equipes/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_DadosValidos_Retorna201()
    {
        // Arrange
        var dto = new EquipeDto { Nome = "Palmeiras", Cidade = "São Paulo", Estado = "SP" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/equipes", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_NomeVazio_Retorna400()
    {
        // Arrange
        var dto = new EquipeDto { Nome = "", Cidade = "São Paulo", Estado = "SP" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/equipes", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_EstadoVazio_Retorna400()
    {
        // Arrange
        var dto = new EquipeDto { Nome = "Atlético", Cidade = "Belo Horizonte", Estado = "" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/equipes", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_IdExistente_Retorna200()
    {
        // Arrange
        var dto = new EquipeDto { IdEquipe = 1, Nome = "Flamengo Atualizado", Cidade = "Rio de Janeiro", Estado = "RJ" };

        // Act
        var response = await _client.PutAsJsonAsync("/api/equipes/1", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_IdInexistente_Retorna404()
    {
        // Arrange
        var dto = new EquipeDto { Nome = "Equipe", Cidade = "Cidade", Estado = "SP" };

        // Act
        var response = await _client.PutAsJsonAsync("/api/equipes/9999", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_IdExistente_Retorna200()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/equipes/2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_IdInexistente_Retorna404()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/equipes/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}