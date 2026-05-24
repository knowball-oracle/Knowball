using System.Net;
using System.Net.Http.Json;
using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Tests.Integration.Fixtures;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Integration.Controllers;

[Collection("Integration")]
public class ArbitroControllerTests : IClassFixture<KnowballWebAppFactory>
{
    private readonly HttpClient _client;

    public ArbitroControllerTests(KnowballWebAppFactory factory)
    {
        _client = factory.CriarClientAutenticado();
    }

    [Fact]
    public async Task GetAll_SemFiltros_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/arbitros");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_IdExistente_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/arbitros/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_IdInexistente_Retorna404()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/arbitros/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_DadosValidos_Retorna201()
    {
        // Arrange
        var dto = new ArbitroDto { Nome = "Carlos Mendes", Status = "Ativo", DataNascimento = new DateTime(1985, 3, 10) };

        // Act
        var response = await _client.PostAsJsonAsync("/api/arbitros", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_StatusInvalido_Retorna400()
    {
        // Arrange
        var dto = new ArbitroDto { Nome = "Carlos Mendes", Status = "Aposentado" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/arbitros", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_IdExistente_Retorna200()
    {
        // Arrange
        var dto = new ArbitroDto { IdArbitro = 1, Nome = "João Silva Atualizado", Status = "Suspenso" };

        // Act
        var response = await _client.PutAsJsonAsync("/api/arbitros/1", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_IdInexistente_Retorna404()
    {
        // Arrange
        var dto = new ArbitroDto { Nome = "Nome", Status = "Ativo" };

        // Act
        var response = await _client.PutAsJsonAsync("/api/arbitros/9999", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_IdExistente_Retorna200()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/arbitros/2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_IdInexistente_Retorna404()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/arbitros/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}