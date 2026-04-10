using System.Net;
using System.Net.Http.Json;
using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Tests.Integration.Fixtures;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Integration.Controllers;

[Collection("Integration")]
public class ParticipacaoControllerTests
{
    private readonly HttpClient _client;

    public ParticipacaoControllerTests(KnowballWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_SemFiltros_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/participacao");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetByIds_IdsExistentes_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/participacao/1/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetByIds_IdsInexistentes_Retorna404()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/participacao/9999/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_TipoValido_Retorna201()
    {
        // Arrange
        var dto = new ParticipacaoDto { IdPartida = 2, IdEquipe = 1, Tipo = "Mandante" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/participacao", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_TipoInvalido_Retorna400()
    {
        // Arrange
        var dto = new ParticipacaoDto { IdPartida = 1, IdEquipe = 1, Tipo = "Casa" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/participacao", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_IdsExistentes_Retorna200()
    {
        // Arrange
        var dto = new ParticipacaoDto { IdPartida = 1, IdEquipe = 2, Tipo = "Visitante" };

        // Act
        var response = await _client.PutAsJsonAsync("/api/participacao/1/2", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_IdsInexistentes_Retorna404()
    {
        // Arrange
        var dto = new ParticipacaoDto { IdPartida = 9999, IdEquipe = 9999, Tipo = "Mandante" };

        // Act
        var response = await _client.PutAsJsonAsync("/api/participacao/9999/9999", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_IdsExistentes_Retorna200()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/participacao/1/2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_IdsInexistentes_Retorna404()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/participacao/9999/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}