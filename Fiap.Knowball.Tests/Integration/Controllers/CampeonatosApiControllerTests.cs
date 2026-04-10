using System.Net;
using System.Net.Http.Json;
using Fiap.Knowball.Tests.Integration.Fixtures;
using Fiap.Knowball.Application.DTOs;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Integration.Controllers;

[Collection("Integration")]
public class CampeonatosApiControllerTests
{
    private readonly HttpClient _client;

    public CampeonatosApiControllerTests(KnowballWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_SemFiltros_Retorna200ComLista()
    {
        // Arrange
        var url = "/api/campeonatos";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<object>();
        body.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_IdExistente_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/campeonatos/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_IdInexistente_Retorna404()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/campeonatos/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_DadosValidos_Retorna201()
    {
        // Arrange
        var dto = new CampeonatoDto { Nome = "Liga Paulista", Categoria = "Sub-17", Ano = 2025 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/campeonatos", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_AnoInvalido_Retorna400()
    {
        // Arrange
        var dto = new CampeonatoDto { Nome = "Campeonato Inválido", Categoria = "Série A", Ano = 1800 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/campeonatos", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_CategoriaVazia_Retorna400()
    {
        // Arrange
        var dto = new CampeonatoDto { Nome = "Campeonato", Categoria = "", Ano = 2025 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/campeonatos", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_IdExistente_Retorna200()
    {
        // Arrange
        var dto = new CampeonatoDto { IdCampeonato = 1, Nome = "Copa Atualizada", Categoria = "Sub-20", Ano = 2025 };

        // Act
        var response = await _client.PutAsJsonAsync("/api/campeonatos/1", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_IdInexistente_Retorna404()
    {
        // Arrange
        var dto = new CampeonatoDto { Nome = "Nome", Categoria = "Série A", Ano = 2025 };

        // Act
        var response = await _client.PutAsJsonAsync("/api/campeonatos/9999", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_IdExistente_Retorna200()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/campeonatos/2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_IdInexistente_Retorna404()
    {
        // Arrange & Act
        var response = await _client.DeleteAsync("/api/campeonatos/9999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Search_ComFiltroNome_Retorna200()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/campeonatos/search?nome=Brasileirão&page=1&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Search_PaginacaoValida_RetornaMetadadosDePaginacao()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/campeonatos/search?page=1&pageSize=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        // ReadFromJsonAsync<dynamic> retorna JsonElement que quebra FluentAssertions
        var body = await response.Content.ReadAsStringAsync();
        body.Should().NotBeNullOrEmpty();
        body.Should().Contain("pagination");
    }
}