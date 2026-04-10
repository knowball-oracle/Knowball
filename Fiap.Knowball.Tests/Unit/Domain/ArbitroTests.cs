using Fiap.Knowball.Models;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Unit.Domain;

public class ArbitroTests
{
    [Theory]
    [InlineData("Ativo")]
    [InlineData("Inativo")]
    [InlineData("Suspenso")]
    public void StatusValido_StatusPermitido_RetornaTrue(string status)
    {
        // Arrange
        var arbitro = new Arbitro { Status = status };

        // Act
        var resultado = arbitro.StatusValido();

        // Assert
        resultado.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Aposentado")]
    [InlineData("ativo")] // case-sensitive
    [InlineData(null)]
    public void StatusValido_StatusInvalido_RetornaFalse(string status)
    {
        // Arrange
        var arbitro = new Arbitro { Status = status };

        // Act
        var resultado = arbitro.StatusValido();

        // Assert
        resultado.Should().BeFalse();
    }
}