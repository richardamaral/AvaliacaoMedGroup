using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvaliacaoMedGrupo.Migrations
{
    /// <inheritdoc />
    public partial class RemoverColunaSexo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove a coluna Sexo legada que existia antes da refatoracao para SexoTipoId
            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Contatos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sexo",
                table: "Contatos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
