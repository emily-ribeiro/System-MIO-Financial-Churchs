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
    public string FormaPagamento { get; set; } = "";
}