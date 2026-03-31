using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AvaliacaoMedGrupo.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTabelaSexoTipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SexoTipoId",
                table: "Contatos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SexoTipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SexoTipos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SexoTipos",
                columns: new[] { "Id", "Descricao" },
                values: new object[,]
                {
                    { 1, "Masculino" },
                    { 2, "Feminino" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_SexoTipoId",
                table: "Contatos",
                column: "SexoTipoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contatos_SexoTipos_SexoTipoId",
                table: "Contatos",
                column: "SexoTipoId",
                principalTable: "SexoTipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contatos_SexoTipos_SexoTipoId",
                table: "Contatos");

            migrationBuilder.DropTable(
                name: "SexoTipos");

            migrationBuilder.DropIndex(
                name: "IX_Contatos_SexoTipoId",
                table: "Contatos");

            migrationBuilder.DropColumn(
                name: "SexoTipoId",
                table: "Contatos");
        }
    }
}
