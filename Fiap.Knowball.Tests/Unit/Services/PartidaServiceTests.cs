using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Exceptions;
using Fiap.Knowball.Application.Services;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.Knowball.Tests.Unit.Services;

public class PartidaServiceTests
{
    private readonly Mock<IPartidaRepository> _repositoryMock;
    private readonly PartidaService _service;

    public PartidaServiceTests()
    {
        _repositoryMock = new Mock<IPartidaRepository>();
        _service = new PartidaService(
            _repositoryMock.Object,
            Mock.Of<ILogger<PartidaService>>());
    }

    [Fact]
    public void CriarPartida_DadosValidos_RetornaDtoComId()
    {
        // Arrange
        var dto = new PartidaDto
        {
            IdCampeonato = 1,
            DataPartida = DateTime.Now.AddDays(7),
            Local = "Maracanã",
            PlacarMandante = 0,
            PlacarVisitante = 0
        };
        _repositoryMock
            .Setup(r => r.Add(It.IsAny<Partida>()))
            .Callback<Partida>(p => p.IdPartida = 10);

        // Act
        var resultado = _service.CriarPartida(dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.IdPartida.Should().Be(10);
        resultado.Local.Should().Be("Maracanã");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Partida>()), Times.Once);
    }

    [Fact]
    public void CriarPartida_PlacarNegativo_LancaBusinessException()
    {
        // Arrange
        var dto = new PartidaDto
        {
            IdCampeonato = 1,
            DataPartida = DateTime.Now.AddDays(7),
            Local = "Arena",
            PlacarMandante = -1,
            PlacarVisitante = 0
        };

        // Act
        var act = () => _service.CriarPartida(dto);

        // Assert
        act.Should().Throw<BusinessException>().WithMessage("*Placar inválido*");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Partida>()), Times.Never);
    }

    [Fact]
    public void CriarPartida_DataPassada_LancaBusinessException()
    {
        // Arrange
        var dto = new PartidaDto
        {
            IdCampeonato = 1,
            DataPartida = DateTime.Now.AddDays(-1),
            Local = "Arena",
            PlacarMandante = 0,
            PlacarVisitante = 0
        };

        // Act
        var act = () => _service.CriarPartida(dto);

        // Assert
        act.Should().Throw<BusinessException>().WithMessage("*Data*inválida*");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Partida>()), Times.Never);
    }

    [Fact]
    public void ObterPorId_PartidaInexistente_RetornaNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99)).Returns((Partida?)null);

        // Act
        var resultado = _service.ObterPorId(99);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public void RemoverPartida_PartidaExistente_ChamaRemoveUmaVez()
    {
        // Arrange
        var partida = new Partida { IdPartida = 1, IdCampeonato = 1, DataPartida = DateTime.Now.AddDays(5), Local = "Arena" };
        _repositoryMock.Setup(r => r.GetById(1)).Returns(partida);

        // Act
        _service.RemoverPartida(1);

        // Assert
        _repositoryMock.Verify(r => r.Remove(1), Times.Once);
    }
}