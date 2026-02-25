using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaIgreja.Migrations
{
    /// <inheritdoc />
    public partial class AjusteModeloPending_FixData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insere uma congregação padrão se não existir
            migrationBuilder.Sql(@"INSERT INTO Congregacoes (Nome, Ativo)
                SELECT 'Sem Congregação', 1
                WHERE NOT EXISTS (SELECT 1 FROM Congregacoes);");

            // Atualiza lançamentos que tenham CongregacaoId NULL para apontar para uma congregação válida
            migrationBuilder.Sql(@"UPDATE Lancamentos
                SET CongregacaoId = (SELECT Id FROM Congregacoes LIMIT 1)
                WHERE CongregacaoId IS NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            // Não desfazemos automaticamente a correção de dados para evitar perda de informação.


        }
    }
}
