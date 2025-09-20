using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Quintamigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JogoUsuario_Jogo_JogosId",
                table: "JogoUsuario");

            migrationBuilder.RenameColumn(
                name: "JogosId",
                table: "JogoUsuario",
                newName: "BibliotecaId");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Jogo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Compra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompradorId = table.Column<int>(type: "int", nullable: false),
                    JogoId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Desconto = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compra_Jogo_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Compra_Usuario_CompradorId",
                        column: x => x.CompradorId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compra_CompradorId",
                table: "Compra",
                column: "CompradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_JogoId",
                table: "Compra",
                column: "JogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_JogoUsuario_Jogo_BibliotecaId",
                table: "JogoUsuario",
                column: "BibliotecaId",
                principalTable: "Jogo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JogoUsuario_Jogo_BibliotecaId",
                table: "JogoUsuario");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Jogo");

            migrationBuilder.RenameColumn(
                name: "BibliotecaId",
                table: "JogoUsuario",
                newName: "JogosId");

            migrationBuilder.AddForeignKey(
                name: "FK_JogoUsuario_Jogo_JogosId",
                table: "JogoUsuario",
                column: "JogosId",
                principalTable: "Jogo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
