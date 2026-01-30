using FinanControl.Core.Enums;

namespace FinanControl.Core.Entities
{
    public class Conta : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Instituicao { get; set; }
        public TipoConta Tipo { get; set; }
        public decimal SaldoAtual { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public string? Cor { get; set; } // Para UI
        public string? Icone { get; set; } // Para UI

        // Relacionamentos
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
    }
}
