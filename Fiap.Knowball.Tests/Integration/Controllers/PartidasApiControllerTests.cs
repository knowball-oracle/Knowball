using System.Net;
using System.Net.Http.Json;
using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Tests.Integration.Fixtures;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Integration.Controllers;

[Collection("Integration")]
public class PartidasApiControllerTests
{
    private readonly HttpClient _client;

    public PartidasApiControllerTests(KnowballWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_SemFiltros_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/partidas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_IdExistente_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/partidas/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_IdInexistente_Retorna404()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/partidas/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_DadosValidos_Retorna201()
    {
        // Arrange
        var dto = new PartidaDto
        {
            IdCampeonato = 1,
            DataPartida = DateTime.Now.AddDays(15),
            Local = "Arena Castelão",
            PlacarMandante = 0,
            PlacarVisitante = 0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/partidas", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_PlacarNegativo_Retorna400()
    {
        // Arrange
        var dto = new PartidaDto
        {
            IdCampeonato = 1,
            DataPartida = DateTime.Now.AddDays(5),
            Local = "Arena",
            PlacarMandante = -1,
            PlacarVisitante = 0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/partidas", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_DataPassada_Retorna400()
    {
        // Arrange
        var dto = new PartidaDto
        {
            IdCampeonato = 1,
            DataPartida = DateTime.Now.AddDays(-3),
            Local = "Arena",
            PlacarMandante = 0,
            PlacarVisitante = 0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/partidas", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_IdExistente_Retorna200()
    {
        // Arrange
        var dto = new PartidaDto
        {
            IdPartida = 1,
            IdCampeonato = 1,
            DataPartida = DateTime.Now.AddDays(20),
            Local = "Maracanã Atualizado",
            PlacarMandante = 0,
            PlacarVisitante = 0
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/partidas/1", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_IdInexistente_Retorna404()
    {
        // Arrange
        var dto = new PartidaDto
        {
            IdCampeonato = 1,
            DataPartida = DateTime.Now.AddDays(5),
            Local = "Arena",
            PlacarMandante = 0,
            PlacarVisitante = 0
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/partidas/9999", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_IdExistente_Retorna200()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/partidas/2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_IdInexistente_Retorna404()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/partidas/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Search_ComFiltroCampeonato_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/partidas/search?idCampeonato=1&page=1&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}