namespace SistemaIgreja.Data
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        
        //'Perfil'
        public string Perfil { get; set; } = string.Empty; // Ex: "Administrador", "Tesoureiro".
        public int? CongregacaoId { get; set; } 
        public bool Ativo { get; set; } = true;
    }
}