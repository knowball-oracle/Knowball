using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Services;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.Knowball.Tests.Unit.Services;

public class ArbitragemServiceTests
{
    private readonly Mock<IArbitragemRepository> _repositoryMock;
    private readonly ArbitragemService _service;

    public ArbitragemServiceTests()
    {
        _repositoryMock = new Mock<IArbitragemRepository>();
        _service = new ArbitragemService(
            _repositoryMock.Object,
            Mock.Of<ILogger<ArbitragemService>>());
    }

    [Fact]
    public void CriarArbitragem_FuncaoValida_ChamaAddUmaVez()
    {
        // Arrange
        var dto = new ArbitragemDto { IdPartida = 1, IdArbitro = 1, Funcao = "Principal" };

        // Act
        var resultado = _service.CriarArbitragem(dto);

        // Assert
        resultado.Should().NotBeNull();
        _repositoryMock.Verify(r => r.Add(It.IsAny<Arbitragem>()), Times.Once);
    }

    [Fact]
    public void CriarArbitragem_FuncaoInvalida_LancaArgumentException()
    {
        // Arrange
        var dto = new ArbitragemDto { IdPartida = 1, IdArbitro = 1, Funcao = "Juiz" };

        // Act
        var act = () => _service.CriarArbitragem(dto);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Função inválida*");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Arbitragem>()), Times.Never);
    }

    [Fact]
    public void ObterPorIds_ArbitragemExistente_RetornaDto()
    {
        // Arrange
        var arbitragem = new Arbitragem { IdPartida = 1, IdArbitro = 2, Funcao = "Assistente 1" };
        _repositoryMock.Setup(r => r.GetByIds(1, 2)).Returns(arbitragem);

        // Act
        var resultado = _service.ObterPorIds(1, 2);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Funcao.Should().Be("Assistente 1");
    }

    [Fact]
    public void ObterPorIds_ArbitragemInexistente_RetornaNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIds(99, 99)).Returns((Arbitragem?)null);

        // Act
        var resultado = _service.ObterPorIds(99, 99);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public void AtualizarArbitragem_ArbitragemInexistente_LancaArgumentException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIds(99, 99)).Returns((Arbitragem?)null);
        var dto = new ArbitragemDto { IdPartida = 99, IdArbitro = 99, Funcao = "Principal" };

        // Act
        var act = () => _service.AtualizarArbitragem(99, 99, dto);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*não encontrada*");
    }
}