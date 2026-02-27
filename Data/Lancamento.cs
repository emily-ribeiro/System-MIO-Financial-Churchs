using System.ComponentModel.DataAnnotations.Schema; 
namespace SistemaIgreja.Data;

public class Lancamento
{
    public int Id { get; set; }
    public int CongregacaoId { get; set; }
    
    
   
    public virtual Congregacao? Congregacao { get; set; } 

    public DateTime DataEvento { get; set; }
    public string Tipo { get; set; } = "";
    public string Categoria { get; set; } = "";
    public decimal Valor { get; set; }
    public string Descricao { get; set; } = "";
    public string Status { get; set; } = "Aberto";
    // Novo campo para controlar o status do repasse entre Matriz e Congregação
    // Valores previstos: null (nenhum / não é repasse), "Pendente", "Aceito", "Recusado"
    public string? RepasseStatus { get; set; }
    public string FormaPagamento { get; set; } = "";
    public int Parcelas { get; set; } = 1;
    public string? CodTransferencia { get; set; }
}