namespace FinanControl.Core.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Endereco { get; set; }
        public string SenhaHash { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; } = true;

        // Relacionamentos
        public ICollection<Conta> Contas { get; set; } = new List<Conta>();
        public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
        public ICollection<FonteRenda> FontesRenda { get; set; } = new List<FonteRenda>();
        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
    }
}
