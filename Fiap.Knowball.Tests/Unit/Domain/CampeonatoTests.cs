using Fiap.Knowball.Models;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Unit.Domain;

public class CampeonatoTests
{
    [Theory]
    [InlineData(2000)]
    [InlineData(2024)]
    [InlineData(2025)]
    public void AnoValido_AnoPermitido_RetornaTrue(int ano)
    {
        // Arrange
        var campeonato = new Campeonato { Ano = ano };

        // Act
        var resultado = campeonato.AnoValido();

        // Assert
        resultado.Should().BeTrue();
    }

    [Theory]
    [InlineData(1800)]
    [InlineData(0)]
    [InlineData(-1)]
    public void AnoValido_AnoInvalido_RetornaFalse(int ano)
    {
        // Arrange
        var campeonato = new Campeonato { Ano = ano };

        // Act
        var resultado = campeonato.AnoValido();

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public void CategoriaValida_CategoriaPreenchida_RetornaTrue()
    {
        // Arrange
        var campeonato = new Campeonato { Categoria = "Sub-20" };

        // Act
        var resultado = campeonato.CategoriaValida();

        // Assert
        resultado.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CategoriaValida_CategoriaVaziaOuNula_RetornaFalse(string categoria)
    {
        // Arrange
        var campeonato = new Campeonato { Categoria = categoria };

        // Act
        var resultado = campeonato.CategoriaValida();

        // Assert
        resultado.Should().BeFalse();
    }
}