namespace FinanControl.Core.Entities
{
    public class Conta : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Instituicao { get; set; }
        public TipoConta Tipo { get; set; }
        public decimal SaldoAtual { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }

        // Relacionamentos
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
    }
}
