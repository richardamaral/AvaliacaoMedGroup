using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvaliacaoMedGrupo.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarSexoNaoEspecificado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SexoTipos",
                columns: new[] { "Id", "Descricao" },
                values: new object[] { 3, "Nao Especificado" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SexoTipos",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
