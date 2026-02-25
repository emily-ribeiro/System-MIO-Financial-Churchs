using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaIgreja.Migrations
{
    /// <inheritdoc />
    public partial class AjusteLancamentoNomeCongregacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeCongregacao",
                table: "Lancamentos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeCongregacao",
                table: "Lancamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
