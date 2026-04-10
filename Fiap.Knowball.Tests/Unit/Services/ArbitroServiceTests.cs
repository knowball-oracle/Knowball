using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Services;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.Knowball.Tests.Unit.Services;

public class ArbitroServiceTests
{
    private readonly Mock<IArbitroRepository> _repositoryMock;
    private readonly ArbitroService _service;

    public ArbitroServiceTests()
    {
        _repositoryMock = new Mock<IArbitroRepository>();
        _service = new ArbitroService(
            _repositoryMock.Object,
            Mock.Of<ILogger<ArbitroService>>());
    }

    [Fact]
    public void CriarArbitro_DadosValidos_RetornaDtoComId()
    {
        // Arrange
        var dto = new ArbitroDto { Nome = "Roberto Carlos", Status = "Ativo", DataNascimento = new DateTime(1980, 5, 10) };
        _repositoryMock
            .Setup(r => r.Add(It.IsAny<Arbitro>()))
            .Callback<Arbitro>(a => a.IdArbitro = 5);

        // Act
        var resultado = _service.CriarArbitro(dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.IdArbitro.Should().Be(5);
        resultado.Nome.Should().Be("Roberto Carlos");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Arbitro>()), Times.Once);
    }

    [Fact]
    public void CriarArbitro_StatusNulo_UsaStatusAtivoPorPadrao()
    {
        // Arrange
        var dto = new ArbitroDto { Nome = "Ana Lima", Status = null };

        // Act
        var resultado = _service.CriarArbitro(dto);

        // Assert
        resultado.Status.Should().Be("Ativo");
    }

    [Fact]
    public void CriarArbitro_StatusInvalido_LancaArgumentException()
    {
        // Arrange
        var dto = new ArbitroDto { Nome = "Paulo Souza", Status = "Aposentado" };

        // Act
        var act = () => _service.CriarArbitro(dto);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Status inválido*");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Arbitro>()), Times.Never);
    }

    [Fact]
    public void ObterPorId_ArbitroExistente_RetornaDto()
    {
        // Arrange
        var arbitro = new Arbitro { IdArbitro = 1, Nome = "João Silva", Status = "Ativo" };
        _repositoryMock.Setup(r => r.GetById(1)).Returns(arbitro);

        // Act
        var resultado = _service.ObterPorId(1);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.IdArbitro.Should().Be(1);
        resultado.Nome.Should().Be("João Silva");
        resultado.Status.Should().Be("Ativo");
    }

    [Fact]
    public void ObterPorId_ArbitroInexistente_RetornaNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99)).Returns((Arbitro?)null);

        // Act
        var resultado = _service.ObterPorId(99);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public void AtualizarArbitro_ArbitroInexistente_LancaArgumentException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99)).Returns((Arbitro?)null);
        var dto = new ArbitroDto { Nome = "Novo Nome", Status = "Ativo" };

        // Act
        var act = () => _service.AtualizarArbitro(99, dto);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*não encontrado*");
        _repositoryMock.Verify(r => r.Update(It.IsAny<Arbitro>()), Times.Never);
    }

    [Fact]
    public void AtualizarArbitro_DadosValidos_ChamaUpdateUmaVez()
    {
        // Arrange
        var arbitro = new Arbitro { IdArbitro = 1, Nome = "Antigo Nome", Status = "Ativo" };
        _repositoryMock.Setup(r => r.GetById(1)).Returns(arbitro);
        var dto = new ArbitroDto { Nome = "Novo Nome", Status = "Suspenso" };

        // Act
        _service.AtualizarArbitro(1, dto);

        // Assert
        _repositoryMock.Verify(r => r.Update(It.IsAny<Arbitro>()), Times.Once);
    }

    [Fact]
    public void RemoverArbitro_ChamaRemoveUmaVez()
    {
        // Arrange & Act
        _service.RemoverArbitro(1);

        // Assert
        _repositoryMock.Verify(r => r.Remove(1), Times.Once);
    }

    [Fact]
    public void ListarArbitros_ComRegistros_RetornaListaMapeada()
    {
        // Arrange
        var arbitros = new List<Arbitro>
        {
            new() { IdArbitro = 1, Nome = "João", Status = "Ativo" },
            new() { IdArbitro = 2, Nome = "Maria", Status = "Inativo" }
        };
        _repositoryMock.Setup(r => r.GetAll()).Returns(arbitros);

        // Act
        var resultado = _service.ListarArbitros().ToList();

        // Assert
        resultado.Should().HaveCount(2);
        resultado[0].Nome.Should().Be("João");
        resultado[1].Status.Should().Be("Inativo");
    }
}