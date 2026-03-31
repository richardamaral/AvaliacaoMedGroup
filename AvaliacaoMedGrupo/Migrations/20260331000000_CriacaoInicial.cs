using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace AvaliacaoMedGrupo.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SexoTipo",
                columns: table => new
                {
                    SexoTipoId = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SexoTipo", x => x.SexoTipoId);
                });

            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    SexoTipoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contatos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contatos_SexoTipo_SexoTipoId",
                        column: x => x.SexoTipoId,
                        principalTable: "SexoTipo",
                        principalColumn: "SexoTipoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SexoTipo",
                columns: new[] { "SexoTipoId", "Descricao" },
                values: new object[,]
                {
                    { 1, "Masculino" },
                    { 2, "Feminino" },
                    { 3, "Nao Especificado" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_SexoTipoId",
                table: "Contatos",
                column: "SexoTipoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Contatos");
            migrationBuilder.DropTable(name: "SexoTipo");
        }
    }
}
