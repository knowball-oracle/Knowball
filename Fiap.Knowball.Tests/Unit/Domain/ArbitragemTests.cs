using Fiap.Knowball.Models;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Unit.Domain;

public class ArbitragemTests
{
    [Theory]
    [InlineData("Principal")]
    [InlineData("Assistente 1")]
    [InlineData("Assistente 2")]
    [InlineData("Quarto Árbitro")]
    public void FuncaoValida_FuncaoPermitida_RetornaTrue(string funcao)
    {
        // Arrange
        var arbitragem = new Arbitragem { Funcao = funcao };

        // Act
        var resultado = arbitragem.FuncaoValida();

        // Assert
        resultado.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Juiz")]
    [InlineData("Assistente")]
    [InlineData(null)]
    public void FuncaoValida_FuncaoInvalida_RetornaFalse(string funcao)
    {
        // Arrange
        var arbitragem = new Arbitragem { Funcao = funcao };

        // Act
        var resultado = arbitragem.FuncaoValida();

        // Assert
        resultado.Should().BeFalse();
    }
}