using Fiap.Knowball.Models;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Unit.Domain;

public class PartidaTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(3, 2)]
    public void PlacarValido_PlacarPositivo_RetornaTrue(int mandante, int visitante)
    {
        // Arrange
        var partida = new Partida { PlacarMandante = mandante, PlacarVisitante = visitante };

        // Act
        var resultado = partida.PlacarValido();

        // Assert
        resultado.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(-2, -3)]
    public void PlacarValido_PlacarNegativo_RetornaFalse(int mandante, int visitante)
    {
        // Arrange
        var partida = new Partida { PlacarMandante = mandante, PlacarVisitante = visitante };

        // Act
        var resultado = partida.PlacarValido();

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public void DataValida_DataFutura_RetornaTrue()
    {
        // Arrange
        var partida = new Partida { DataPartida = DateTime.Now.AddDays(1) };

        // Act
        var resultado = partida.DataValida();

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public void DataValida_DataPassada_RetornaFalse()
    {
        // Arrange
        var partida = new Partida { DataPartida = DateTime.Now.AddDays(-1) };

        // Act
        var resultado = partida.DataValida();

        // Assert
        resultado.Should().BeFalse();
    }
}