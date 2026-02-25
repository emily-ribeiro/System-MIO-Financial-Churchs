using System.ComponentModel.DataAnnotations.Schema; 
namespace SistemaIgreja.Data;

public class Lancamento
{
    public int Id { get; set; }
    public DateTime DataEvento { get; set; } = DateTime.Now;
    public string Tipo { get; set; } = "Entrada"; 
    public string Categoria { get; set; } = "";    
    public decimal Valor { get; set; }
    public string Descricao { get; set; } = "";
    
    //(Parcelas e Status)
    public string FormaPagamento { get; set; } = "À Vista"; 
    public int Parcelas { get; set; } = 1;
    public string Status { get; set; } = "Enviado"; 

    // (Para o Relatório e para o Login)
    public int? CongregacaoId { get; set; } 
    public string? NomeCongregacao { get; set; } 
}