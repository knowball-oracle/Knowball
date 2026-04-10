using Fiap.Knowball.Models;
using FluentAssertions;

namespace Fiap.Knowball.Tests.Unit.Domain;

public class EquipeTests
{
    [Fact]
    public void DadosValidos_TodosCamposPreenchidos_RetornaTrue()
    {
        // Arrange
        var equipe = new Equipe { Nome = "Flamengo", Cidade = "Rio de Janeiro", Estado = "RJ" };

        // Act
        var resultado = equipe.DadosValidos();

        // Assert
        resultado.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "São Paulo", "SP")]
    [InlineData("   ", "São Paulo", "SP")]
    [InlineData(null, "São Paulo", "SP")]
    public void DadosValidos_NomeVazioOuNulo_RetornaFalse(string nome, string cidade, string estado)
    {
        // Arrange
        var equipe = new Equipe { Nome = nome, Cidade = cidade, Estado = estado };

        // Act
        var resultado = equipe.DadosValidos();

        // Assert
        resultado.Should().BeFalse();
    }

    [Theory]
    [InlineData("Corinthians", "", "SP")]
    [InlineData("Corinthians", "   ", "SP")]
    [InlineData("Corinthians", null, "SP")]
    public void DadosValidos_CidadeVaziaOuNula_RetornaFalse(string nome, string cidade, string estado)
    {
        // Arrange
        var equipe = new Equipe { Nome = nome, Cidade = cidade, Estado = estado };

        // Act
        var resultado = equipe.DadosValidos();

        // Assert
        resultado.Should().BeFalse();
    }

    [Theory]
    [InlineData("Palmeiras", "São Paulo", "")]
    [InlineData("Palmeiras", "São Paulo", "   ")]
    [InlineData("Palmeiras", "São Paulo", null)]
    public void DadosValidos_EstadoVazioOuNulo_RetornaFalse(string nome, string cidade, string estado)
    {
        // Arrange
        var equipe = new Equipe { Nome = nome, Cidade = cidade, Estado = estado };

        // Act
        var resultado = equipe.DadosValidos();

        // Assert
        resultado.Should().BeFalse();
    }
}