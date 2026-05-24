using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Exceptions;
using Fiap.Knowball.Application.Services;
using Fiap.Knowball.Domain.Repositories;
using Fiap.Knowball.Models;
using Fiap.Knowball.Models.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fiap.Knowball.Tests.Unit.Services;

public class DenunciaServiceTests
{
    private readonly Mock<IDenunciaRepository> _repositoryMock = new();
    private readonly Mock<IDenunciaLogRepository> _logRepositoryMock = new();
    private readonly Mock<ILogger<DenunciaService>> _loggerMock = new();
    private readonly DenunciaService _service;

    public DenunciaServiceTests()
    {
        _service = new DenunciaService(
            _repositoryMock.Object,
            _logRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public void CriarDenuncia_DadosValidos_RetornaDtoComId()
    {
        // Arrange
        var dto = new DenunciaDto
        {
            IdPartida = 1,
            IdArbitro = 1,
            Protocolo = "PROT-2025-001",
            Relato = "Árbitro cometeu erro grave.",
            DataDenuncia = DateTime.Now,
            Status = "Em Análise" // único status válido junto com "Resolvida"
        };
        _repositoryMock
            .Setup(r => r.Add(It.IsAny<Denuncia>()))
            .Callback<Denuncia>(d => d.IdDenuncia = 10);

        // Act
        var resultado = _service.CriarDenuncia(dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.IdDenuncia.Should().Be(10);
        resultado.Protocolo.Should().Be("PROT-2025-001");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Denuncia>()), Times.Once);
    }

    [Fact]
    public void CriarDenuncia_StatusResolvida_CriaComSucesso()
    {
        // Arrange
        var dto = new DenunciaDto
        {
            IdPartida = 1,
            IdArbitro = 1,
            Protocolo = "PROT-2025-002",
            Relato = "Relato válido.",
            DataDenuncia = DateTime.Now,
            Status = "Resolvida"
        };

        // Act
        var act = () => _service.CriarDenuncia(dto);

        // Assert
        act.Should().NotThrow();
        _repositoryMock.Verify(r => r.Add(It.IsAny<Denuncia>()), Times.Once);
    }

    [Fact]
    public void CriarDenuncia_StatusInvalido_LancaBusinessException()
    {
        // Arrange
        var dto = new DenunciaDto
        {
            Protocolo = "PROT-001",
            Relato = "Relato válido.",
            Status = "Aberta", // inválido — serviço aceita apenas "Em Análise" e "Resolvida"
            DataDenuncia = DateTime.Now
        };

        // Act
        var act = () => _service.CriarDenuncia(dto);

        // Assert
        act.Should().Throw<BusinessException>().WithMessage("*Status inválido*");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Denuncia>()), Times.Never);
    }

    [Fact]
    public void CriarDenuncia_ProtocoloVazio_LancaBusinessException()
    {
        // Arrange — Status válido para passar na primeira validação
        var dto = new DenunciaDto
        {
            Protocolo = "",
            Relato = "Relato válido.",
            Status = "Em Análise",
            DataDenuncia = DateTime.Now
        };

        // Act
        var act = () => _service.CriarDenuncia(dto);

        // Assert
        act.Should().Throw<BusinessException>().WithMessage("*Protocolo inválido*");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Denuncia>()), Times.Never);
    }

    [Fact]
    public void CriarDenuncia_RelatoVazio_LancaBusinessException()
    {
        // Arrange — Status e Protocolo válidos para chegar na validação do Relato
        var dto = new DenunciaDto
        {
            Protocolo = "PROT-001",
            Relato = "",
            Status = "Em Análise",
            DataDenuncia = DateTime.Now
        };

        // Act
        var act = () => _service.CriarDenuncia(dto);

        // Assert
        act.Should().Throw<BusinessException>().WithMessage("*Relato inválido*");
        _repositoryMock.Verify(r => r.Add(It.IsAny<Denuncia>()), Times.Never);
    }

    [Fact]
    public void ObterPorId_DenunciaExistente_RetornaDto()
    {
        // Arrange
        var denuncia = new Denuncia
        {
            IdDenuncia = 1,
            Protocolo = "PROT-001",
            Relato = "Relato.",
            Status = "Em Análise",
            DataDenuncia = DateTime.Now
        };
        _repositoryMock.Setup(r => r.GetById(1)).Returns(denuncia);

        // Act
        var resultado = _service.ObterPorId(1);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Protocolo.Should().Be("PROT-001");
        resultado.Status.Should().Be("Em Análise");
    }

    [Fact]
    public void ObterPorId_DenunciaInexistente_RetornaNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99)).Returns((Denuncia?)null);

        // Act
        var resultado = _service.ObterPorId(99);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public void AtualizarDenuncia_DenunciaInexistente_LancaBusinessException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99)).Returns((Denuncia?)null);
        var dto = new DenunciaDto
        {
            Protocolo = "PROT-002",
            Relato = "Novo relato.",
            Status = "Em Análise"
        };

        // Act
        var act = () => _service.AtualizarDenuncia(99, dto);

        // Assert
        act.Should().Throw<BusinessException>().WithMessage("*não encontrada*");
        _repositoryMock.Verify(r => r.Update(It.IsAny<Denuncia>()), Times.Never);
    }

    [Fact]
    public void AtualizarDenuncia_DadosValidos_ChamaUpdateUmaVez()
    {
        // Arrange
        var denuncia = new Denuncia
        {
            IdDenuncia = 1,
            Protocolo = "PROT-001",
            Relato = "Relato original.",
            Status = "Em Análise"
        };
        _repositoryMock.Setup(r => r.GetById(1)).Returns(denuncia);
        var dto = new DenunciaDto
        {
            Protocolo = "PROT-001",
            Relato = "Relato atualizado.",
            Status = "Resolvida"
        };

        // Act
        _service.AtualizarDenuncia(1, dto);

        // Assert
        _repositoryMock.Verify(r => r.Update(It.IsAny<Denuncia>()), Times.Once);
    }

    [Fact]
    public void RemoverDenuncia_ChamaRemoveUmaVez()
    {
        // Arrange & Act
        _service.RemoverDenuncia(1);

        // Assert
        _repositoryMock.Verify(r => r.Remove(1), Times.Once);
    }

    [Fact]
    public void ListarDenuncias_SemRegistros_RetornaListaVazia()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAll()).Returns(new List<Denuncia>());

        // Act
        var resultado = _service.ListarDenuncias();

        // Assert
        resultado.Should().BeEmpty();
    }
}