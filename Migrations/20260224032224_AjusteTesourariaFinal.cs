using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaIgreja.Migrations
{
    /// <inheritdoc />
    public partial class AjusteTesourariaFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CongregacaoId",
                table: "Usuarios",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeCongregacao",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CongregacaoId",
                table: "Lancamentos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Lancamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FormaPagamento",
                table: "Lancamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeCongregacao",
                table: "Lancamentos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Parcelas",
                table: "Lancamentos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Lancamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CongregacaoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "NomeCongregacao",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "FormaPagamento",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "NomeCongregacao",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "Parcelas",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Lancamentos");

            migrationBuilder.AlterColumn<int>(
                name: "CongregacaoId",
                table: "Lancamentos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
