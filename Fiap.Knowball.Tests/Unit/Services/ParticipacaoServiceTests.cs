using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Services;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.Knowball.Tests.Unit.Services;

public class ParticipacaoServiceTests
{
    private readonly Mock<IParticipacaoRepository> _repositoryMock;
    private readonly ParticipacaoService _service;

    public ParticipacaoServiceTests()
    {
        _repositoryMock = new Mock<IParticipacaoRepository>();
        _service = new ParticipacaoService(
            _repositoryMock.Object,
            Mock.Of<ILogger<ParticipacaoService>>());
    }

    [Theory]
    [InlineData("Mandante")]
    [InlineData("Visitante")]
    public void CriarParticipacao_TipoValido_ChamaAddUmaVez(string tipo)
    {
        // Arrange
        var dto = new ParticipacaoDto { IdPartida = 1, IdEquipe = 1, Tipo = tipo };

        // Act
        var resultado = _service.CriarParticipacao(dto);

        // Assert
        resultado.Should().NotBeNull();
        _repositoryMock.Verify(r => r.Add(It.IsAny<Participacao>()), Times.Once);
    }

    [Fact]
    public void CriarParticipacao_TipoInvalido_LancaArgumentException()
    {
        // Arrange
        var dto = new ParticipacaoDto { IdPartida = 1, IdEquipe = 1, Tipo = "Casa" };

        // Act
        var act = () => _service.CriarParticipacao(dto);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*inválido*");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Participacao>()), Times.Never);
    }

    [Fact]
    public void ObterPorIds_ParticipacaoExistente_RetornaDto()
    {
        // Arrange
        var participacao = new Participacao { IdPartida = 1, IdEquipe = 2, Tipo = "Mandante" };
        _repositoryMock.Setup(r => r.GetByIds(1, 2)).Returns(participacao);

        // Act
        var resultado = _service.ObterPorIds(1, 2);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Tipo.Should().Be("Mandante");
        resultado.IdPartida.Should().Be(1);
        resultado.IdEquipe.Should().Be(2);
    }

    [Fact]
    public void ObterPorIds_ParticipacaoInexistente_RetornaNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIds(99, 99)).Returns((Participacao?)null);

        // Act
        var resultado = _service.ObterPorIds(99, 99);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public void AtualizarParticipacao_ParticipacaoInexistente_LancaArgumentException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIds(99, 99)).Returns((Participacao?)null);
        var dto = new ParticipacaoDto { IdPartida = 99, IdEquipe = 99, Tipo = "Visitante" };

        // Act
        var act = () => _service.AtualizarParticipacao(99, 99, dto);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*não encontrada*");
        _repositoryMock.Verify(r => r.Update(It.IsAny<Participacao>()), Times.Never);
    }

    [Fact]
    public void AtualizarParticipacao_TipoInvalido_LancaArgumentException()
    {
        // Arrange
        var participacao = new Participacao { IdPartida = 1, IdEquipe = 1, Tipo = "Mandante" };
        _repositoryMock.Setup(r => r.GetByIds(1, 1)).Returns(participacao);
        var dto = new ParticipacaoDto { IdPartida = 1, IdEquipe = 1, Tipo = "Fora de Casa" };

        // Act
        var act = () => _service.AtualizarParticipacao(1, 1, dto);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*inválido*");
        _repositoryMock.Verify(r => r.Update(It.IsAny<Participacao>()), Times.Never);
    }

    [Fact]
    public void RemoverParticipacao_ChamaRemoveUmaVez()
    {
        // Arrange & Act
        _service.RemoverParticipacao(1, 2);

        // Assert
        _repositoryMock.Verify(r => r.Remove(1, 2), Times.Once);
    }
}