using Fiap.Knowball.Models;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Unit.Domain;

public class DenunciaTests
{
    [Theory]
    [InlineData("Em Análise")]
    [InlineData("Resolvida")]
    public void StatusValido_StatusPermitido_RetornaTrue(string status)
    {
        // Arrange
        var denuncia = new Denuncia { Status = status };

        // Act
        var resultado = denuncia.StatusValido();

        // Assert
        resultado.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Aberta")]
    [InlineData("Encerrada")]
    [InlineData("Pendente")]
    [InlineData("em análise")] // case-sensitive
    public void StatusValido_StatusInvalido_RetornaFalse(string status)
    {
        // Arrange
        var denuncia = new Denuncia { Status = status };

        // Act
        var resultado = denuncia.StatusValido();

        // Assert
        resultado.Should().BeFalse();
    }

    [Theory]
    [InlineData("Procedente")]
    [InlineData("Improcedente")]
    [InlineData("Inconclusiva")]
    [InlineData(null)]
    public void ResultadoAnaliseValido_ValorPermitido_RetornaTrue(string resultado)
    {
        // Arrange
        var denuncia = new Denuncia { ResultadoAnalise = resultado };

        // Act
        var result = denuncia.ResultadoAnaliseValido();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Aprovado")]
    [InlineData("Negado")]
    [InlineData("procedente")] // case-sensitive
    public void ResultadoAnaliseValido_ValorInvalido_RetornaFalse(string resultado)
    {
        // Arrange
        var denuncia = new Denuncia { ResultadoAnalise = resultado };

        // Act
        var result = denuncia.ResultadoAnaliseValido();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ProtocoloValido_ProtocoloPreenchido_RetornaTrue()
    {
        // Arrange
        var denuncia = new Denuncia { Protocolo = "PROT-2025-001" };

        // Act
        var resultado = denuncia.ProtocoloValido();

        // Assert
        resultado.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ProtocoloValido_ProtocoloVazioOuNulo_RetornaFalse(string protocolo)
    {
        // Arrange
        var denuncia = new Denuncia { Protocolo = protocolo };

        // Act
        var resultado = denuncia.ProtocoloValido();

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public void RelatoValido_RelatoPreenchido_RetornaTrue()
    {
        // Arrange
        var denuncia = new Denuncia { Relato = "O árbitro cometeu um erro grave no segundo tempo." };

        // Act
        var resultado = denuncia.RelatoValido();

        // Assert
        resultado.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void RelatoValido_RelatoVazioOuNulo_RetornaFalse(string relato)
    {
        // Arrange
        var denuncia = new Denuncia { Relato = relato };

        // Act
        var resultado = denuncia.RelatoValido();

        // Assert
        resultado.Should().BeFalse();
    }
}