using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Exceptions;
using Fiap.Knowball.Application.Services;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.Knowball.Tests.Unit.Services;

public class CampeonatoServiceTests
{
    private readonly Mock<ICampeonatoRepository> _repositoryMock;
    private readonly CampeonatoService _service;

    public CampeonatoServiceTests()
    {
        _repositoryMock = new Mock<ICampeonatoRepository>();
        _service = new CampeonatoService(
            _repositoryMock.Object,
            Mock.Of<ILogger<CampeonatoService>>());
    }

    [Fact]
    public void CriarCampeonato_DadosValidos_RetornaDtoComId()
    {
        // Arrange
        var dto = new CampeonatoDto
        {
            Nome = "Campeonato FIAP",
            Categoria = "Sub-17",
            Ano = 2025
        };
        _repositoryMock
            .Setup(r => r.Add(It.IsAny<Campeonato>()))
            .Callback<Campeonato>(c => c.IdCampeonato = 5);

        // Act
        var resultado = _service.CriarCampeonato(dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.IdCampeonato.Should().Be(5);
        resultado.Nome.Should().Be("Campeonato FIAP");
        resultado.Categoria.Should().Be("Sub-17");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Campeonato>()), Times.Once);
    }

    [Theory]
    [InlineData("Sub-13")]
    [InlineData("Sub-15")]
    [InlineData("Sub-17")]
    [InlineData("Sub-20")]
    public void CriarCampeonato_CategoriasValidas_NaoLancaExcecao(string categoria)
    {
        // Arrange
        var dto = new CampeonatoDto { Nome = "Torneio", Categoria = categoria, Ano = 2025 };

        // Act
        var act = () => _service.CriarCampeonato(dto);

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("Série A")]
    [InlineData("Nacional")]
    [InlineData("")]
    [InlineData(null)]
    public void CriarCampeonato_CategoriaInvalida_LancaBusinessException(string categoria)
    {
        // Arrange
        var dto = new CampeonatoDto { Nome = "Torneio", Categoria = categoria, Ano = 2025 };

        // Act
        var act = () => _service.CriarCampeonato(dto);

        // Assert
        act.Should().Throw<BusinessException>().WithMessage("*inválidos*");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Campeonato>()), Times.Never);
    }

    [Theory]
    [InlineData(1899)]
    [InlineData(2027)]
    public void CriarCampeonato_AnoInvalido_LancaBusinessException(int ano)
    {
        // Arrange
        var dto = new CampeonatoDto { Nome = "Torneio", Categoria = "Sub-17", Ano = ano };

        // Act
        var act = () => _service.CriarCampeonato(dto);

        // Assert
        act.Should().Throw<BusinessException>().WithMessage("*inválidos*");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Campeonato>()), Times.Never);
    }

    [Fact]
    public void ListarCampeonatos_SemRegistros_RetornaListaVazia()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAll()).Returns(new List<Campeonato>());

        // Act
        var resultado = _service.ListarCampeonatos();

        // Assert
        resultado.Should().BeEmpty();
    }

    [Fact]
    public void ListarCampeonatos_ComRegistros_RetornaLista()
    {
        // Arrange
        var campeonatos = new List<Campeonato>
        {
            new() { IdCampeonato = 1, Nome = "Copa Sub-17", Categoria = "Sub-17", Ano = 2025 },
            new() { IdCampeonato = 2, Nome = "Copa Sub-20", Categoria = "Sub-20", Ano = 2024 }
        };
        _repositoryMock.Setup(r => r.GetAll()).Returns(campeonatos);

        // Act
        var resultado = _service.ListarCampeonatos().ToList();

        // Assert
        resultado.Should().HaveCount(2);
        resultado[0].Nome.Should().Be("Copa Sub-17");
        resultado[1].Categoria.Should().Be("Sub-20");
    }

    [Fact]
    public void ObterPorId_CampeonatoExistente_RetornaDto()
    {
        // Arrange
        var campeonato = new Campeonato
        {
            IdCampeonato = 1,
            Nome = "Copa Sub-17",
            Categoria = "Sub-17",
            Ano = 2025
        };
        _repositoryMock.Setup(r => r.GetById(1)).Returns(campeonato);

        // Act
        var resultado = _service.ObterPorId(1);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Categoria.Should().Be("Sub-17");
        resultado.Ano.Should().Be(2025);
    }

    [Fact]
    public void ObterPorId_CampeonatoInexistente_RetornaNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99)).Returns((Campeonato?)null);

        // Act
        var resultado = _service.ObterPorId(99);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public void AtualizarCampeonato_DadosValidos_ChamaUpdateUmaVez()
    {
        // Arrange
        var campeonato = new Campeonato
        {
            IdCampeonato = 1,
            Nome = "Copa Antiga",
            Categoria = "Sub-15",
            Ano = 2024
        };
        _repositoryMock.Setup(r => r.GetById(1)).Returns(campeonato);
        var dto = new CampeonatoDto
        {
            Nome = "Copa Atualizada",
            Categoria = "Sub-20",
            Ano = 2025
        };

        // Act
        _service.AtualizarCampeonato(1, dto);

        // Assert
        _repositoryMock.Verify(r => r.Update(It.IsAny<Campeonato>()), Times.Once);
    }

    [Fact]
    public void AtualizarCampeonato_CampeonatoInexistente_LancaBusinessException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99)).Returns((Campeonato?)null);
        var dto = new CampeonatoDto { Nome = "X", Categoria = "Sub-17", Ano = 2025 };

        // Act
        var act = () => _service.AtualizarCampeonato(99, dto);

        // Assert
        act.Should().Throw<BusinessException>().WithMessage("*não encontrado*");
        _repositoryMock.Verify(r => r.Update(It.IsAny<Campeonato>()), Times.Never);
    }

    [Fact]
    public void RemoverCampeonato_CampeonatoExistente_ChamaRemoveUmaVez()
    {
        // Arrange
        var campeonato = new Campeonato
        {
            IdCampeonato = 1,
            Nome = "Copa Sub-17",
            Categoria = "Sub-17",
            Ano = 2025
        };
        _repositoryMock.Setup(r => r.GetById(1)).Returns(campeonato);

        // Act
        _service.RemoverCampeonato(1);

        // Assert
        _repositoryMock.Verify(r => r.Remove(1), Times.Once);
    }

    [Fact]
    public void RemoverCampeonato_CampeonatoInexistente_LancaBusinessException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99)).Returns((Campeonato?)null);

        // Act
        var act = () => _service.RemoverCampeonato(99);

        // Assert
        act.Should().Throw<BusinessException>().WithMessage("*não encontrado*");
        _repositoryMock.Verify(r => r.Remove(It.IsAny<int>()), Times.Never);
    }
}