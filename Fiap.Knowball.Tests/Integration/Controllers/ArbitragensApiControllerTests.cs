using System.Net;
using System.Net.Http.Json;
using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Tests.Integration.Fixtures;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Integration.Controllers;

[Collection("Integration")]
public class ArbitragensApiControllerTests
{
    private readonly HttpClient _client;

    public ArbitragensApiControllerTests(KnowballWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_SemFiltros_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/arbitragens");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetByIds_IdsExistentes_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/arbitragens/1/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetByIds_IdsInexistentes_Retorna404()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/arbitragens/9999/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_FuncaoValida_Retorna201()
    {
        // Arrange
        var dto = new ArbitragemDto { IdPartida = 1, IdArbitro = 2, Funcao = "Assistente 1" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/arbitragens", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_FuncaoInvalida_Retorna400()
    {
        // Arrange
        var dto = new ArbitragemDto { IdPartida = 2, IdArbitro = 1, Funcao = "Juiz" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/arbitragens", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delete_IdsExistentes_Retorna200()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/arbitragens/2/2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_IdsInexistentes_Retorna404()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/arbitragens/9999/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}