using Fiap.Knowball.Models;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Unit.Domain;

public class ParticipacaoTests
{
    [Theory]
    [InlineData("Mandante")]
    [InlineData("Visitante")]
    public void TipoValido_TipoPermitido_RetornaTrue(string tipo)
    {
        // Arrange
        var participacao = new Participacao { Tipo = tipo };

        // Act
        var resultado = participacao.TipoValido();

        // Assert
        resultado.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Casa")]
    [InlineData("mandante")] // case-sensitive
    [InlineData(null)]
    public void TipoValido_TipoInvalido_RetornaFalse(string tipo)
    {
        // Arrange
        var participacao = new Participacao { Tipo = tipo };

        // Act
        var resultado = participacao.TipoValido();

        // Assert
        resultado.Should().BeFalse();
    }
}