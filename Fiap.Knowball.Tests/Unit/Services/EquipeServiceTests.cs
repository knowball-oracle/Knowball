using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Services;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.Knowball.Tests.Unit.Services;

public class EquipeServiceTests
{
    private readonly Mock<IEquipeRepository> _repositoryMock;
    private readonly EquipeService _service;

    public EquipeServiceTests()
    {
        _repositoryMock = new Mock<IEquipeRepository>();
        _service = new EquipeService(
            _repositoryMock.Object,
            Mock.Of<ILogger<EquipeService>>());
    }

    [Fact]
    public void CriarEquipe_DadosValidos_RetornaDtoComId()
    {
        // Arrange
        var dto = new EquipeDto { Nome = "Flamengo", Cidade = "Rio de Janeiro", Estado = "RJ" };
        _repositoryMock
            .Setup(r => r.Add(It.IsAny<Equipe>()))
            .Callback<Equipe>(e => e.IdEquipe = 3);

        // Act
        var resultado = _service.CriarEquipe(dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.IdEquipe.Should().Be(3);
        resultado.Nome.Should().Be("Flamengo");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Equipe>()), Times.Once);
    }

    [Fact]
    public void CriarEquipe_DadosInvalidos_LancaArgumentException()
    {
        // Arrange
        var dto = new EquipeDto { Nome = "", Cidade = "São Paulo", Estado = "SP" };

        // Act
        var act = () => _service.CriarEquipe(dto);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*inválidos*");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Equipe>()), Times.Never);
    }

    [Fact]
    public void ObterPorId_EquipeExistente_RetornaDto()
    {
        // Arrange
        var equipe = new Equipe { IdEquipe = 1, Nome = "Corinthians", Cidade = "São Paulo", Estado = "SP" };
        _repositoryMock.Setup(r => r.GetById(1)).Returns(equipe);

        // Act
        var resultado = _service.ObterPorId(1);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.IdEquipe.Should().Be(1);
        resultado.Nome.Should().Be("Corinthians");
        resultado.Estado.Should().Be("SP");
    }

    [Fact]
    public void ObterPorId_EquipeInexistente_RetornaNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99)).Returns((Equipe?)null);

        // Act
        var resultado = _service.ObterPorId(99);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public void AtualizarEquipe_EquipeInexistente_LancaArgumentException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99)).Returns((Equipe?)null);
        var dto = new EquipeDto { Nome = "Novo Nome", Cidade = "Curitiba", Estado = "PR" };

        // Act
        var act = () => _service.AtualizarEquipe(99, dto);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*não encontrada*");
        _repositoryMock.Verify(r => r.Update(It.IsAny<Equipe>()), Times.Never);
    }

    [Fact]
    public void AtualizarEquipe_DadosValidos_ChamaUpdateUmaVez()
    {
        // Arrange
        var equipe = new Equipe { IdEquipe = 1, Nome = "Antigo", Cidade = "BH", Estado = "MG" };
        _repositoryMock.Setup(r => r.GetById(1)).Returns(equipe);
        var dto = new EquipeDto { Nome = "Atletico MG", Cidade = "Belo Horizonte", Estado = "MG" };

        // Act
        _service.AtualizarEquipe(1, dto);

        // Assert
        _repositoryMock.Verify(r => r.Update(It.IsAny<Equipe>()), Times.Once);
    }

    [Fact]
    public void RemoverEquipe_ChamaRemoveUmaVez()
    {
        // Arrange & Act
        _service.RemoverEquipe(1);

        // Assert
        _repositoryMock.Verify(r => r.Remove(1), Times.Once);
    }

    [Fact]
    public void ListarEquipes_ComDoisRegistros_RetornaListaComDoisItens()
    {
        // Arrange
        var equipes = new List<Equipe>
        {
            new() { IdEquipe = 1, Nome = "Flamengo", Cidade = "Rio de Janeiro", Estado = "RJ" },
            new() { IdEquipe = 2, Nome = "Palmeiras", Cidade = "São Paulo", Estado = "SP" }
        };
        _repositoryMock.Setup(r => r.GetAll()).Returns(equipes);

        // Act
        var resultado = _service.ListarEquipes().ToList();

        // Assert
        resultado.Should().HaveCount(2);
        resultado.Should().Contain(e => e.Nome == "Flamengo");
        resultado.Should().Contain(e => e.Nome == "Palmeiras");
    }
}