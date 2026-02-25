namespace SistemaIgreja.Data
{
    public class Congregacao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        
        //Define se a igreja está ativa ou fechada
        public bool Ativo { get; set; } = true; 
    }
}