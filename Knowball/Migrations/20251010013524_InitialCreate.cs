using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Knowball.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Arbitragens",
                columns: table => new
                {
                    IdPartida = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IdArbitro = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Funcao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arbitragens", x => new { x.IdPartida, x.IdArbitro });
                });

            migrationBuilder.CreateTable(
                name: "Arbitros",
                columns: table => new
                {
                    IdArbitro = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Status = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false, defaultValueSql: "'Ativo'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arbitros", x => x.IdArbitro);
                });

            migrationBuilder.CreateTable(
                name: "Campeonatos",
                columns: table => new
                {
                    IdCampeonato = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Categoria = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    Ano = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campeonatos", x => x.IdCampeonato);
                });

            migrationBuilder.CreateTable(
                name: "Denuncias",
                columns: table => new
                {
                    IdDenuncia = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    IdPartida = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IdArbitro = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Protocolo = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Relato = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataDenuncia = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false, defaultValueSql: "'Em Análise'"),
                    ResultadoAnalise = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denuncias", x => x.IdDenuncia);
                });

            migrationBuilder.CreateTable(
                name: "Equipes",
                columns: table => new
                {
                    IdEquipe = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Cidade = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Estado = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipes", x => x.IdEquipe);
                });

            migrationBuilder.CreateTable(
                name: "Partidas",
                columns: table => new
                {
                    IdPartida = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    IdCampeonato = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataPartida = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Local = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PlacarMandante = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PlacarVisitante = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidas", x => x.IdPartida);
                });

            migrationBuilder.CreateTable(
                name: "Participacoes",
                columns: table => new
                {
                    IdPartida = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IdEquipe = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Tipo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participacoes", x => new { x.IdPartida, x.IdEquipe });
                    table.ForeignKey(
                        name: "FK_Participacoes_Partidas_IdPartida",
                        column: x => x.IdPartida,
                        principalTable: "Partidas",
                        principalColumn: "IdPartida",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_Protocolo",
                table: "Denuncias",
                column: "Protocolo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arbitragens");

            migrationBuilder.DropTable(
                name: "Arbitros");

            migrationBuilder.DropTable(
                name: "Campeonatos");

            migrationBuilder.DropTable(
                name: "Denuncias");

            migrationBuilder.DropTable(
                name: "Equipes");

            migrationBuilder.DropTable(
                name: "Participacoes");

            migrationBuilder.DropTable(
                name: "Partidas");
        }
    }
}
